using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Application.Abstractions;
using DemoCICD.Infrastructure.Authentication;
using DemoCICD.Infrastructure.Caching;
using DemoCICD.Infrastructure.DependencyInjection.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoCICD.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        // Add infrastructure services here
        services.AddTransient<IJwtTokenService, JwtTokenService>();
        services.AddTransient<ITokenCacheService, TokenCacheService>();
        services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();
        services.AddTransient<IUserManagementService, UserManagementService>();
        services.AddTransient<IRoleManagementService, RoleManagementService>();
        services.AddTransient<IPermissionManagementService, PermissionManagementService>();
    }

    public static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisOptions = new RedisOptions();
        configuration.GetSection(nameof(RedisOptions)).Bind(redisOptions);

        if (!string.IsNullOrEmpty(redisOptions.ConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisOptions.ConnectionString;
                options.InstanceName = redisOptions.InstanceName;
            });
        }
        else
        {
            // Fallback to in-memory cache if Redis is not configured
            services.AddDistributedMemoryCache();
        }
    }
}
