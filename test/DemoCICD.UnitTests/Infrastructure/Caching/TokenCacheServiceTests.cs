using DemoCICD.Infrastructure.Caching;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text;

namespace DemoCICD.UnitTests.Infrastructure.Caching;

public class TokenCacheServiceTests
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<TokenCacheService> _logger;
    private readonly TokenCacheService _tokenCacheService;

    public TokenCacheServiceTests()
    {
        _distributedCache = Substitute.For<IDistributedCache>();
        _logger = Substitute.For<ILogger<TokenCacheService>>();
        _tokenCacheService = new TokenCacheService(_distributedCache, _logger);
    }

    [Fact]
    public async Task GetRefreshTokenAsync_WithValidUserId_ReturnsToken()
    {
        // Arrange
        var userId = "test-user-id";
        var expectedToken = "refresh-token-value";
        var cacheKey = $"refresh_token:{userId}";
        var tokenBytes = Encoding.UTF8.GetBytes(expectedToken);

        _distributedCache.GetAsync(cacheKey, default).Returns(tokenBytes);

        // Act
        var result = await _tokenCacheService.GetRefreshTokenAsync(userId);

        // Assert
        result.Should().Be(expectedToken);
        await _distributedCache.Received(1).GetAsync(cacheKey, default);
    }

    [Fact]
    public async Task GetRefreshTokenAsync_WhenTokenNotFound_ReturnsNull()
    {
        // Arrange
        var userId = "test-user-id";
        var cacheKey = $"refresh_token:{userId}";

        _distributedCache.GetAsync(cacheKey, default).Returns((byte[]?)null);

        // Act
        var result = await _tokenCacheService.GetRefreshTokenAsync(userId);

        // Assert
        result.Should().BeNull();
        await _distributedCache.Received(1).GetAsync(cacheKey, default);
    }

    [Fact]
    public async Task GetRefreshTokenAsync_WhenExceptionThrown_ReturnsNull()
    {
        // Arrange
        var userId = "test-user-id";
        var cacheKey = $"refresh_token:{userId}";

        _distributedCache.When(x => x.GetAsync(cacheKey, default))
            .Do(x => throw new Exception("Cache error"));

        // Act
        var result = await _tokenCacheService.GetRefreshTokenAsync(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task SetRefreshTokenAsync_WithValidParameters_SetsTokenInCache()
    {
        // Arrange
        var userId = "test-user-id";
        var refreshToken = "refresh-token-value";
        var expiration = TimeSpan.FromHours(1);
        var cacheKey = $"refresh_token:{userId}";

        // Act
        await _tokenCacheService.SetRefreshTokenAsync(userId, refreshToken, expiration);

        // Assert
        await _distributedCache.Received(1).SetAsync(
            cacheKey,
            Arg.Any<byte[]>(),
            Arg.Is<DistributedCacheEntryOptions>(options =>
                options.AbsoluteExpirationRelativeToNow == expiration),
            default);
    }

    [Fact]
    public async Task SetRefreshTokenAsync_WhenExceptionThrown_HandlesGracefully()
    {
        // Arrange
        var userId = "test-user-id";
        var refreshToken = "refresh-token-value";
        var expiration = TimeSpan.FromHours(1);

        _distributedCache.When(x => x.SetAsync(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>(), default))
            .Do(x => throw new Exception("Cache error"));

        // Act & Assert
        await _tokenCacheService.Invoking(x => x.SetRefreshTokenAsync(userId, refreshToken, expiration))
            .Should().NotThrowAsync();
    }

    [Fact]
    public async Task RemoveRefreshTokenAsync_WithValidUserId_RemovesTokenFromCache()
    {
        // Arrange
        var userId = "test-user-id";
        var cacheKey = $"refresh_token:{userId}";

        // Act
        await _tokenCacheService.RemoveRefreshTokenAsync(userId);

        // Assert
        await _distributedCache.Received(1).RemoveAsync(cacheKey, default);
    }

    [Fact]
    public async Task RemoveRefreshTokenAsync_WhenExceptionThrown_HandlesGracefully()
    {
        // Arrange
        var userId = "test-user-id";
        var cacheKey = $"refresh_token:{userId}";

        _distributedCache.When(x => x.RemoveAsync(cacheKey, default))
            .Do(x => throw new Exception("Cache error"));

        // Act & Assert
        await _tokenCacheService.Invoking(x => x.RemoveRefreshTokenAsync(userId))
            .Should().NotThrowAsync();
    }

    [Fact]
    public async Task BlacklistTokenAsync_WithValidParameters_BlacklistsToken()
    {
        // Arrange
        var tokenId = "token-id";
        var expiration = TimeSpan.FromHours(1);
        var cacheKey = $"blacklist_token:{tokenId}";

        // Act
        await _tokenCacheService.BlacklistTokenAsync(tokenId, expiration);

        // Assert
        await _distributedCache.Received(1).SetAsync(
            cacheKey,
            Arg.Any<byte[]>(),
            Arg.Is<DistributedCacheEntryOptions>(options =>
                options.AbsoluteExpirationRelativeToNow == expiration),
            default);
    }

    [Fact]
    public async Task BlacklistTokenAsync_WhenExceptionThrown_HandlesGracefully()
    {
        // Arrange
        var tokenId = "token-id";
        var expiration = TimeSpan.FromHours(1);

        _distributedCache.When(x => x.SetAsync(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>(), default))
            .Do(x => throw new Exception("Cache error"));

        // Act & Assert
        await _tokenCacheService.Invoking(x => x.BlacklistTokenAsync(tokenId, expiration))
            .Should().NotThrowAsync();
    }

    [Fact]
    public async Task IsTokenBlacklistedAsync_WithBlacklistedToken_ReturnsTrue()
    {
        // Arrange
        var tokenId = "token-id";
        var cacheKey = $"blacklist_token:{tokenId}";
        var tokenBytes = Encoding.UTF8.GetBytes("blacklisted");

        _distributedCache.GetAsync(cacheKey, default).Returns(tokenBytes);

        // Act
        var result = await _tokenCacheService.IsTokenBlacklistedAsync(tokenId);

        // Assert
        result.Should().BeTrue();
        await _distributedCache.Received(1).GetAsync(cacheKey, default);
    }

    [Fact]
    public async Task IsTokenBlacklistedAsync_WithNonBlacklistedToken_ReturnsFalse()
    {
        // Arrange
        var tokenId = "token-id";
        var cacheKey = $"blacklist_token:{tokenId}";

        _distributedCache.GetAsync(cacheKey, default).Returns((byte[]?)null);

        // Act
        var result = await _tokenCacheService.IsTokenBlacklistedAsync(tokenId);

        // Assert
        result.Should().BeFalse();
        await _distributedCache.Received(1).GetAsync(cacheKey, default);
    }

    [Fact]
    public async Task IsTokenBlacklistedAsync_WhenExceptionThrown_ReturnsFalse()
    {
        // Arrange
        var tokenId = "token-id";
        var cacheKey = $"blacklist_token:{tokenId}";

        _distributedCache.When(x => x.GetAsync(cacheKey, default))
            .Do(x => throw new Exception("Cache error"));

        // Act
        var result = await _tokenCacheService.IsTokenBlacklistedAsync(tokenId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task RemoveAllUserTokensAsync_WithValidUserId_RemovesAllTokens()
    {
        // Arrange
        var userId = "test-user-id";
        var refreshTokenKey = $"refresh_token:{userId}";

        // Act
        await _tokenCacheService.RemoveAllUserTokensAsync(userId);

        // Assert
        await _distributedCache.Received(1).RemoveAsync(refreshTokenKey, default);
    }

    [Fact]
    public async Task RemoveAllUserTokensAsync_WhenExceptionThrown_HandlesGracefully()
    {
        // Arrange
        var userId = "test-user-id";

        _distributedCache.When(x => x.RemoveAsync(Arg.Any<string>(), default))
            .Do(x => throw new Exception("Cache error"));

        // Act & Assert
        await _tokenCacheService.Invoking(x => x.RemoveAllUserTokensAsync(userId))
            .Should().NotThrowAsync();
    }
}