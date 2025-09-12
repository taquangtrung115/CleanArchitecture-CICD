using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

public class LogoutCommandHandler : ICommandHandler<Command.Logout>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ITokenCacheService _tokenCacheService;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(
        IJwtTokenService jwtTokenService,
        ITokenCacheService tokenCacheService,
        ILogger<LogoutCommandHandler> logger)
    {
        _jwtTokenService = jwtTokenService;
        _tokenCacheService = tokenCacheService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.Logout request, CancellationToken cancellationToken)
    {
        try
        {
            // Get token ID from the access token
            var tokenId = _jwtTokenService.GetTokenIdFromToken(request.AccessToken);
            if (string.IsNullOrEmpty(tokenId))
            {
                return Result.Failure(new Error("Authentication.InvalidToken", "Invalid token"));
            }

            // Get user ID from token claims
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                // Remove refresh token from Redis
                await _tokenCacheService.RemoveRefreshTokenAsync(userId);
            }

            // Blacklist the access token for the remaining of its lifetime
            // Assuming access token expires in 15 minutes (as per JWT configuration)
            await _tokenCacheService.BlacklistTokenAsync(tokenId, TimeSpan.FromMinutes(15));

            _logger.LogInformation("User {UserId} logged out successfully", userId);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return Result.Failure(new Error("Authentication.LogoutError", "An error occurred during logout"));
        }
    }
}