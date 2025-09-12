using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using DemoCICD.Application.Abstractions;

namespace DemoCICD.Infrastructure.Caching;

public class TokenCacheService : ITokenCacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<TokenCacheService> _logger;
    private const string RefreshTokenPrefix = "refresh_token:";
    private const string BlacklistTokenPrefix = "blacklist_token:";

    public TokenCacheService(IDistributedCache distributedCache, ILogger<TokenCacheService> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<string?> GetRefreshTokenAsync(string userId)
    {
        try
        {
            var key = $"{RefreshTokenPrefix}{userId}";
            var token = await _distributedCache.GetStringAsync(key);
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting refresh token for user {UserId}", userId);
            return null;
        }
    }

    public async Task SetRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiration)
    {
        try
        {
            var key = $"{RefreshTokenPrefix}{userId}";
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            await _distributedCache.SetStringAsync(key, refreshToken, options);
            _logger.LogInformation("Refresh token set for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting refresh token for user {UserId}", userId);
        }
    }

    public async Task RemoveRefreshTokenAsync(string userId)
    {
        try
        {
            var key = $"{RefreshTokenPrefix}{userId}";
            await _distributedCache.RemoveAsync(key);
            _logger.LogInformation("Refresh token removed for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing refresh token for user {UserId}", userId);
        }
    }

    public async Task BlacklistTokenAsync(string tokenId, TimeSpan expiration)
    {
        try
        {
            var key = $"{BlacklistTokenPrefix}{tokenId}";
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            await _distributedCache.SetStringAsync(key, "blacklisted", options);
            _logger.LogInformation("Token {TokenId} blacklisted", tokenId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error blacklisting token {TokenId}", tokenId);
        }
    }

    public async Task<bool> IsTokenBlacklistedAsync(string tokenId)
    {
        try
        {
            var key = $"{BlacklistTokenPrefix}{tokenId}";
            var result = await _distributedCache.GetStringAsync(key);
            return !string.IsNullOrEmpty(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if token {TokenId} is blacklisted", tokenId);
            return false;
        }
    }

    public async Task RemoveAllUserTokensAsync(string userId)
    {
        try
        {
            await RemoveRefreshTokenAsync(userId);
            _logger.LogInformation("All tokens removed for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing all tokens for user {UserId}", userId);
        }
    }
}