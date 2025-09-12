using DemoCICD.Application.Abstractions;
using DemoCICD.Infrastructure.Authentication;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoCICD.UnitTests.Infrastructure.Authentication;

public class JwtTokenServiceTests
{
    private readonly IConfiguration _configuration;
    private readonly ITokenCacheService _tokenCacheService;
    private readonly JwtTokenService _jwtTokenService;

    public JwtTokenServiceTests()
    {
        _tokenCacheService = Substitute.For<ITokenCacheService>();

        // Create a real configuration for testing
        var configurationData = new Dictionary<string, string?>
        {
            {"JwtOption:SecretKey", "MySecretKeyForJwtTokenThatNeedsToBeAtLeast32CharactersLong"},
            {"JwtOption:Issuer", "TestIssuer"},
            {"JwtOption:Audience", "TestAudience"},
            {"JwtOption:ExpireMin", "60"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationData)
            .Build();

        _jwtTokenService = new JwtTokenService(_configuration, _tokenCacheService);
    }

    [Fact]
    public void GenerateAccessToken_WithValidClaims_ReturnsValidToken()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "User")
        };

        // Act
        var token = _jwtTokenService.GenerateAccessToken(claims);

        // Assert
        token.Should().NotBeNullOrEmpty();
        
        // Verify token structure
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadJwtToken(token);
        
        jsonToken.Should().NotBeNull();
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "testuser");
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == "test@example.com");
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "User");
        jsonToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Jti);
        jsonToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Iat);
    }

    [Fact]
    public void GenerateAccessToken_WithEmptyClaims_ReturnsValidToken()
    {
        // Arrange
        var claims = new List<Claim>();

        // Act
        var token = _jwtTokenService.GenerateAccessToken(claims);

        // Assert
        token.Should().NotBeNullOrEmpty();
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadJwtToken(token);
        
        jsonToken.Should().NotBeNull();
        jsonToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Jti);
        jsonToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Iat);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsValidBase64Token()
    {
        // Act
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        // Assert
        refreshToken.Should().NotBeNullOrEmpty();
        
        // Verify it's valid base64
        var bytes = Convert.FromBase64String(refreshToken);
        bytes.Should().NotBeEmpty();
        bytes.Length.Should().Be(32); // 32 bytes as defined in the service
    }

    [Fact]
    public void GenerateRefreshToken_GeneratesUniqueTokens()
    {
        // Act
        var token1 = _jwtTokenService.GenerateRefreshToken();
        var token2 = _jwtTokenService.GenerateRefreshToken();

        // Assert
        token1.Should().NotBeNullOrEmpty();
        token2.Should().NotBeNullOrEmpty();
        token1.Should().NotBe(token2);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_WithValidExpiredToken_ReturnsPrincipal()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Email, "test@example.com")
        };
        
        // Generate a token first
        var token = _jwtTokenService.GenerateAccessToken(claims);

        // Act
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(token);

        // Assert
        principal.Should().NotBeNull();
        principal.Identity.Should().NotBeNull();
        principal.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "testuser");
        principal.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == "test@example.com");
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_WithInvalidToken_ThrowsException()
    {
        // Arrange
        var invalidToken = "invalid-token";

        // Act & Assert
        _jwtTokenService.Invoking(x => x.GetPrincipalFromExpiredToken(invalidToken))
            .Should().Throw<Exception>();
    }

    [Fact]
    public void GetTokenIdFromToken_WithValidToken_ReturnsTokenId()
    {
        // Arrange
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "testuser") };
        var token = _jwtTokenService.GenerateAccessToken(claims);

        // Act
        var tokenId = _jwtTokenService.GetTokenIdFromToken(token);

        // Assert
        tokenId.Should().NotBeNullOrEmpty();
        Guid.TryParse(tokenId, out _).Should().BeTrue(); // Should be a valid GUID
    }

    [Fact]
    public void GetTokenIdFromToken_WithInvalidToken_ReturnsNull()
    {
        // Arrange
        var invalidToken = "invalid-token";

        // Act
        var tokenId = _jwtTokenService.GetTokenIdFromToken(invalidToken);

        // Assert
        tokenId.Should().BeNull();
    }

    [Fact]
    public void GetTokenIdFromToken_WithNullToken_ReturnsNull()
    {
        // Arrange
        string nullToken = null;

        // Act
        var tokenId = _jwtTokenService.GetTokenIdFromToken(nullToken);

        // Assert
        tokenId.Should().BeNull();
    }

    [Fact]
    public async Task ValidateTokenAsync_WithValidToken_ReturnsTrue()
    {
        // Arrange
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "testuser") };
        var token = _jwtTokenService.GenerateAccessToken(claims);
        var tokenId = _jwtTokenService.GetTokenIdFromToken(token);

        _tokenCacheService.IsTokenBlacklistedAsync(tokenId).Returns(false);

        // Act
        var isValid = await _jwtTokenService.ValidateTokenAsync(token);

        // Assert
        isValid.Should().BeTrue();
        await _tokenCacheService.Received(1).IsTokenBlacklistedAsync(tokenId);
    }

    [Fact]
    public async Task ValidateTokenAsync_WithBlacklistedToken_ReturnsFalse()
    {
        // Arrange
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "testuser") };
        var token = _jwtTokenService.GenerateAccessToken(claims);
        var tokenId = _jwtTokenService.GetTokenIdFromToken(token);

        _tokenCacheService.IsTokenBlacklistedAsync(tokenId).Returns(true);

        // Act
        var isValid = await _jwtTokenService.ValidateTokenAsync(token);

        // Assert
        isValid.Should().BeFalse();
        await _tokenCacheService.Received(1).IsTokenBlacklistedAsync(tokenId);
    }

    [Fact]
    public async Task ValidateTokenAsync_WithInvalidToken_ReturnsFalse()
    {
        // Arrange
        var invalidToken = "invalid-token";

        // Act
        var isValid = await _jwtTokenService.ValidateTokenAsync(invalidToken);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateTokenAsync_WithTokenWithoutId_ReturnsFalse()
    {
        // Arrange
        // Create a token manually without JTI claim
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyForJwtTokenThatNeedsToBeAtLeast32CharactersLong"));
        var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: "TestIssuer",
            audience: "TestAudience",
            claims: new[] { new Claim(ClaimTypes.Name, "testuser") },
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: signInCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        // Act
        var isValid = await _jwtTokenService.ValidateTokenAsync(tokenString);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateTokenAsync_WhenCacheThrowsException_ReturnsFalse()
    {
        // Arrange
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "testuser") };
        var token = _jwtTokenService.GenerateAccessToken(claims);
        var tokenId = _jwtTokenService.GetTokenIdFromToken(token);

        _tokenCacheService.When(x => x.IsTokenBlacklistedAsync(tokenId))
            .Do(x => throw new Exception("Cache error"));

        // Act
        var isValid = await _jwtTokenService.ValidateTokenAsync(token);

        // Assert
        isValid.Should().BeFalse();
    }
}