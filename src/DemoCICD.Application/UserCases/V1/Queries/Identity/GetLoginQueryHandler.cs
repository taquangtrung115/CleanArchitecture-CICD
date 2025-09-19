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

namespace DemoCICD.Application.UserCases.V1.Queries.Identity;

public sealed class GetLoginQueryHandler : IQueryHandler<Query.Login, Response.Authenticated>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ITokenCacheService _tokenCacheService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly ILogger<GetLoginQueryHandler> _logger;

    public GetLoginQueryHandler(
        IJwtTokenService jwtTokenService,
        ITokenCacheService tokenCacheService,
        IUserAuthenticationService userAuthenticationService,
        ILogger<GetLoginQueryHandler> logger)
    {
        _jwtTokenService = jwtTokenService;
        _tokenCacheService = tokenCacheService;
        _userAuthenticationService = userAuthenticationService;
        _logger = logger;
    }

    public async Task<Result<Response.Authenticated>> Handle(Query.Login request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate user credentials
            var authResult = await _userAuthenticationService.ValidateUserAsync(request.UserName, request.Password);
            if (!authResult.IsSuccess)
            {
                return Result.Failure<Response.Authenticated>(
                    new Error("Authentication.InvalidCredentials", authResult.ErrorMessage ?? "Invalid username or password"));
            }

            // Generate claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authResult.UserId!),
                new Claim(ClaimTypes.Name, authResult.UserName!),
                new Claim(ClaimTypes.Email, authResult.Email ?? string.Empty),
                new Claim("FullName", authResult.FullName ?? string.Empty)
            };

            // Add user roles to claims
            var userRoles = await _userAuthenticationService.GetUserRolesAsync(authResult.UserId!);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Generate tokens
            var accessToken = _jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            // Store refresh token in Redis with 7 days expiration
            var refreshTokenExpiration = TimeSpan.FromDays(7);
            await _tokenCacheService.SetRefreshTokenAsync(authResult.UserId!, refreshToken, refreshTokenExpiration);

            _logger.LogInformation("User {UserId} logged in successfully", authResult.UserId);

            var response = new Response.Authenticated(
                accessToken,
                refreshToken,
                DateTime.UtcNow.Add(refreshTokenExpiration)
            );

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {UserName}", request.UserName);
            return Result.Failure<Response.Authenticated>(
                new Error("Authentication.LoginError", "An error occurred during login"));
        }
    }
}
