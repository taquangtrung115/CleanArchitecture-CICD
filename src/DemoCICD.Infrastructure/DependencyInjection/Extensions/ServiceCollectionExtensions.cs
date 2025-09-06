using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Application.Abstractions;
using DemoCICD.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DemoCICD.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        // Add infrastructure services here
        services.AddTransient<IJwtTokenService, JwtTokenService>();
    }
}
