
using FluentValidation;
using DemoCICD.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using DemoCICD.Application.Mapper;

namespace DemoCICD.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigureMediatR(this IServiceCollection services) =>
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
        .AddValidatorsFromAssembly(
            DemoCICD.Contract.AssemblyReference.Assembly
            , includeInternalTypes: true);

    public static IServiceCollection AddConfigureAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(typeof(ServiceProfile));
}
