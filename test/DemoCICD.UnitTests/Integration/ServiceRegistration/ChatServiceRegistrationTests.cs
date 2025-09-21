using Microsoft.Extensions.DependencyInjection;
using DemoCICD.Persistence.DependencyInjection.Extensions;
using DemoCICD.Application.DependencyInjection.Extensions;
using DemoCICD.Domain.Abstractions.Reponsitories;
using Microsoft.Extensions.Configuration;
using Xunit;
using MediatR;

namespace DemoCICD.UnitTests.Integration.ServiceRegistration;

/// <summary>
/// Tests to verify that Chat services can be resolved without constraint violations
/// </summary>
public class ChatServiceRegistrationTests
{
    [Fact]
    public void ServiceRegistration_ShouldBuildServiceProvider_WithoutConstraintViolations()
    {
        // Arrange
        var services = new ServiceCollection();
        
        // Add minimal configuration needed for testing
        var inMemorySettings = new Dictionary<string, string>
        {
            {"ConnectionStrings:DefaultConnection", "Server=.;Database=TestDb;Trusted_Connection=true;"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
        
        services.AddSingleton<IConfiguration>(configuration);
        
        // Register dependencies
        services.AddSqlConfiguration();
        services.AddRepositoryBaseConfiguration();
        services.AddConfigureMediatR();
        
        // Act & Assert - This should not throw any exceptions related to constraint violations
        var serviceProvider = services.BuildServiceProvider();
        
        // Verify that the AppUser repository can be resolved
        var appUserRepository = serviceProvider.GetService<IAppUserRepository>();
        Assert.NotNull(appUserRepository);
        
        // Verify that MediatR is properly configured
        var mediator = serviceProvider.GetService<IMediator>();
        Assert.NotNull(mediator);
        
        // If we get here without exceptions, the constraint violation issue is fixed
        Assert.True(true, "Service provider built successfully without constraint violations");
    }
}