using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

public class RefreshTokenCommandHandler : ICommandHandler<Command.RefreshTokenRequest, Response.Authenticated>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ITokenCacheService _tokenCacheService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IJwtTokenService jwtTokenService,
        ITokenCacheService tokenCacheService,
        IUserAuthenticationService userAuthenticationService,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _jwtTokenService = jwtTokenService;
        _tokenCacheService = tokenCacheService;
        _userAuthenticationService = userAuthenticationService;
        _logger = logger;
    }

    public async Task<Result<Response.Authenticated>> Handle(Command.RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate the expired access token
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return Result.Failure<Response.Authenticated>(
                    new Error("Authentication.InvalidToken", "Invalid access token"));
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Result.Failure<Response.Authenticated>(
                    new Error("Authentication.InvalidToken", "Invalid user ID in token"));
            }

            // Validate refresh token from Redis
            var storedRefreshToken = await _tokenCacheService.GetRefreshTokenAsync(userId);
            if (string.IsNullOrEmpty(storedRefreshToken) || storedRefreshToken != request.RefreshToken)
            {
                return Result.Failure<Response.Authenticated>(
                    new Error("Authentication.InvalidRefreshToken", "Invalid or expired refresh token"));
            }

            // Get user information and roles
            var userName = principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
            var email = principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            var fullName = principal.FindFirst("FullName")?.Value ?? string.Empty;

            // Generate new claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email),
                new Claim("FullName", fullName)
            };

            // Add user roles to claims
            var userRoles = await _userAuthenticationService.GetUserRolesAsync(userId);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Generate new tokens
            var newAccessToken = _jwtTokenService.GenerateAccessToken(claims);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            // Blacklist the old access token
            var oldTokenId = _jwtTokenService.GetTokenIdFromToken(request.AccessToken);
            if (!string.IsNullOrEmpty(oldTokenId))
            {
                await _tokenCacheService.BlacklistTokenAsync(oldTokenId, TimeSpan.FromMinutes(15));
            }

            // Store new refresh token in Redis
            var refreshTokenExpiration = TimeSpan.FromDays(7);
            await _tokenCacheService.SetRefreshTokenAsync(userId, newRefreshToken, refreshTokenExpiration);

            _logger.LogInformation("Token refreshed successfully for user {UserId}", userId);

            var response = new Response.Authenticated(
                newAccessToken,
                newRefreshToken,
                DateTime.UtcNow.Add(refreshTokenExpiration)
            );

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return Result.Failure<Response.Authenticated>(
                new Error("Authentication.RefreshError", "An error occurred during token refresh"));
        }
    }
}