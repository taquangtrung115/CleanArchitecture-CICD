using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DemoCICD.API.Middleware;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenValidationMiddleware> _logger;

    public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IJwtTokenService jwtTokenService, ITokenCacheService tokenCacheService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var tokenId = jwtTokenService.GetTokenIdFromToken(token);
                if (!string.IsNullOrEmpty(tokenId))
                {
                    var isBlacklisted = await tokenCacheService.IsTokenBlacklistedAsync(tokenId);
                    if (isBlacklisted)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token has been invalidated");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token in middleware");
            }
        }

        await _next(context);
    }
}