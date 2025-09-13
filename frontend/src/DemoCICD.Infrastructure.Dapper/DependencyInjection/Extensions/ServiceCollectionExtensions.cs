
using DemoCICD.Domain.Abstractions.Dappers;
using DemoCICD.Domain.Abstractions.Dappers.Repositories.Product;
using DemoCICD.Infrastructure.Dapper.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DemoCICD.Infrastructure.Dapper.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureDapper(this IServiceCollection services)
        => services.AddTransient<IProductRepository, ProductRepository>()
            .AddTransient<IUnitOfWork, UnitOfWork>();
}
