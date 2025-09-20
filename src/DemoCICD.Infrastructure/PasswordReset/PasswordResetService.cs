using DemoCICD.Application.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace DemoCICD.Infrastructure.PasswordReset;

public class PasswordResetService : IPasswordResetService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<PasswordResetService> _logger;
    private const int CodeExpirationMinutes = 15;
    private const int CodeLength = 6;

    public PasswordResetService(IMemoryCache cache, ILogger<PasswordResetService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<string> GenerateResetCodeAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            // Generate a random 6-digit code
            var code = GenerateRandomCode();
            var cacheKey = GetCacheKey(email);

            // Store the code in cache with expiration
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CodeExpirationMinutes)
            };

            _cache.Set(cacheKey, code, cacheOptions);

            _logger.LogInformation("Reset code generated for email: {Email}, expires in {Minutes} minutes", 
                email, CodeExpirationMinutes);

            await Task.CompletedTask;
            return code;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating reset code for email: {Email}", email);
            throw;
        }
    }

    public async Task<bool> VerifyResetCodeAsync(string email, string code, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = GetCacheKey(email);
            
            if (_cache.TryGetValue(cacheKey, out string? storedCode))
            {
                var isValid = string.Equals(storedCode, code, StringComparison.Ordinal);
                
                if (isValid)
                {
                    _logger.LogInformation("Reset code verified successfully for email: {Email}", email);
                }
                else
                {
                    _logger.LogWarning("Invalid reset code provided for email: {Email}", email);
                }

                await Task.CompletedTask;
                return isValid;
            }

            _logger.LogWarning("No reset code found or code expired for email: {Email}", email);
            await Task.CompletedTask;
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying reset code for email: {Email}", email);
            return false;
        }
    }

    public async Task<bool> InvalidateResetCodeAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = GetCacheKey(email);
            _cache.Remove(cacheKey);

            _logger.LogInformation("Reset code invalidated for email: {Email}", email);
            
            await Task.CompletedTask;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating reset code for email: {Email}", email);
            return false;
        }
    }

    private static string GenerateRandomCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    private static string GetCacheKey(string email)
    {
        return $"password_reset_{email.ToLowerInvariant()}";
    }
}