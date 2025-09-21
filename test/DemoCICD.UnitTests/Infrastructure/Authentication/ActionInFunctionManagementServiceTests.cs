using Microsoft.EntityFrameworkCore;
using DemoCICD.Infrastructure.Authentication;
using DemoCICD.Persistence;
using DemoCICD.Domain.Entities.Identity;
using Action = DemoCICD.Domain.Entities.Identity.Action;
using Function = DemoCICD.Domain.Entities.Identity.Function;

namespace DemoCICD.UnitTests.Infrastructure.Authentication;

public class ActionInFunctionManagementServiceTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CreateActionInFunctionAsync_WithValidData_ReturnsSuccessResult()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ActionInFunctionManagementService(context);

        // Seed test data
        context.Actions.Add(new Action { Id = "READ", Name = "Read Action", IsActive = true });
        context.Functions.Add(new Function { Id = "USER", Name = "User Management", Url = "/users", IsActive = true });
        await context.SaveChangesAsync();

        // Act
        var result = await service.CreateActionInFunctionAsync("READ", "USER");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("READ", result.Value.ActionId);
        Assert.Equal("USER", result.Value.FunctionId);

        // Verify it's saved in database
        var savedActionInFunction = await context.ActionInFunctions
            .FirstOrDefaultAsync(af => af.ActionId == "READ" && af.FunctionId == "USER");
        Assert.NotNull(savedActionInFunction);
    }

    [Fact]
    public async Task CreateActionInFunctionAsync_WithDuplicateData_ReturnsNull()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ActionInFunctionManagementService(context);

        // Seed test data
        context.Actions.Add(new Action { Id = "READ", Name = "Read Action", IsActive = true });
        context.Functions.Add(new Function { Id = "USER", Name = "User Management", Url = "/users", IsActive = true });
        context.ActionInFunctions.Add(new ActionInFunction { ActionId = "READ", FunctionId = "USER" });
        await context.SaveChangesAsync();

        // Act
        var result = await service.CreateActionInFunctionAsync("READ", "USER");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetActionInFunctionsAsync_WithValidData_ReturnsPagedResults()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ActionInFunctionManagementService(context);

        // Seed test data
        context.Actions.AddRange(
            new Action { Id = "READ", Name = "Read Action", IsActive = true },
            new Action { Id = "WRITE", Name = "Write Action", IsActive = true }
        );
        context.Functions.AddRange(
            new Function { Id = "USER", Name = "User Management", Url = "/users", IsActive = true },
            new Function { Id = "ROLE", Name = "Role Management", Url = "/roles", IsActive = true }
        );
        context.ActionInFunctions.AddRange(
            new ActionInFunction { ActionId = "READ", FunctionId = "USER" },
            new ActionInFunction { ActionId = "WRITE", FunctionId = "USER" },
            new ActionInFunction { ActionId = "READ", FunctionId = "ROLE" }
        );
        await context.SaveChangesAsync();

        // Act
        var (actionInFunctions, totalCount) = await service.GetActionInFunctionsAsync(1, 10);

        // Assert
        Assert.Equal(3, totalCount);
        Assert.Equal(3, actionInFunctions.Count());
    }

    [Fact]
    public async Task DeleteActionInFunctionAsync_WithValidData_ReturnsTrue()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ActionInFunctionManagementService(context);

        // Seed test data
        context.Actions.Add(new Action { Id = "READ", Name = "Read Action", IsActive = true });
        context.Functions.Add(new Function { Id = "USER", Name = "User Management", Url = "/users", IsActive = true });
        context.ActionInFunctions.Add(new ActionInFunction { ActionId = "READ", FunctionId = "USER" });
        await context.SaveChangesAsync();

        // Act
        var result = await service.DeleteActionInFunctionAsync("READ", "USER");

        // Assert
        Assert.True(result);

        // Verify it's deleted from database
        var deletedActionInFunction = await context.ActionInFunctions
            .FirstOrDefaultAsync(af => af.ActionId == "READ" && af.FunctionId == "USER");
        Assert.Null(deletedActionInFunction);
    }

    [Fact]
    public async Task DeleteActionInFunctionAsync_WithNonExistentData_ReturnsFalse()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ActionInFunctionManagementService(context);

        // Act
        var result = await service.DeleteActionInFunctionAsync("NONEXISTENT", "USER");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ActionInFunctionExistsAsync_WithExistingData_ReturnsTrue()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ActionInFunctionManagementService(context);

        // Seed test data
        context.ActionInFunctions.Add(new ActionInFunction { ActionId = "READ", FunctionId = "USER" });
        await context.SaveChangesAsync();

        // Act
        var result = await service.ActionInFunctionExistsAsync("READ", "USER");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ActionInFunctionExistsAsync_WithNonExistentData_ReturnsFalse()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ActionInFunctionManagementService(context);

        // Act
        var result = await service.ActionInFunctionExistsAsync("NONEXISTENT", "USER");

        // Assert
        Assert.False(result);
    }
}