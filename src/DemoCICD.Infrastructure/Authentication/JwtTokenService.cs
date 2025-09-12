using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Application.Abstractions;
using DemoCICD.Infrastructure.DependencyInjection.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DemoCICD.Infrastructure.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtOption jwtOption = new JwtOption();
    private readonly ITokenCacheService _tokenCacheService;

    public JwtTokenService(IConfiguration configuration, ITokenCacheService tokenCacheService)
    {
        configuration.GetSection(nameof(JwtOption)).Bind(jwtOption);
        _tokenCacheService = tokenCacheService;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var tokenId = Guid.NewGuid().ToString();
        var claimsList = claims.ToList();
        claimsList.Add(new Claim(JwtRegisteredClaimNames.Jti, tokenId));
        claimsList.Add(new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecretKey));
        var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtOption.Issuer,
            audience: jwtOption.Audience,
            claims: claimsList,
            expires: DateTime.Now.AddMinutes(jwtOption.ExpireMin),
            signingCredentials: signInCredentials
            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false, // We're checking an expired token
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOption.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    public string? GetTokenIdFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);
            return jsonToken.Claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenId = GetTokenIdFromToken(token);
            if (string.IsNullOrEmpty(tokenId))
                return false;

            // Check if token is blacklisted
            var isBlacklisted = await _tokenCacheService.IsTokenBlacklistedAsync(tokenId);
            if (isBlacklisted)
                return false;

            // Validate token signature and expiration
            var key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOption.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
