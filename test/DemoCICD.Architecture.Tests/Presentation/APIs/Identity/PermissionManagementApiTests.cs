using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Presentation.APIs.Identity;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using static DemoCICD.Contract.Services.V1.Identity.Response;

namespace DemoCICD.Architecture.Presentation.APIs.Identity;

public class PermissionManagementApiTests
{
    private readonly ISender _sender;

    public PermissionManagementApiTests()
    {
        _sender = Substitute.For<ISender>();
    }

    [Fact]
    public async Task CreatePermissionV1_WithValidCommand_ReturnsCreatedResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var command = new Command.CreatePermission(
            roleId,
            "PRODUCT_MANAGEMENT",
            "READ");

        var expectedResponse = Result.Success(new Response.PermissionCreated(
            roleId,
            "PRODUCT_MANAGEMENT",
            "READ"));

        _sender.Send(command).Returns(expectedResponse);

        // Act
        var result = await PermissionManagementApi.CreatePermissionV1(_sender, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task CreatePermissionV1_WithFailedCommand_ReturnsFailureResult()
    {
        // Arrange
        var command = new Command.CreatePermission(
            Guid.Empty,
            "",
            "");

        var errorResponse = Result.Failure<Response.PermissionCreated>(new Error("PERMISSION.INVALID_DATA", "Invalid permission data"));
        _sender.Send(command).Returns(errorResponse);

        // Act
        var result = await PermissionManagementApi.CreatePermissionV1(_sender, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task GetPermissionsV1_WithDefaultParameters_ReturnsOkResult()
    {
        // Arrange
        var expectedResponse = Result.Success(new Response.PermissionList(
            new List<PermissionSummary>(),
            0,
            1,
            10));

        _sender.Send(Arg.Any<Query.GetPermissions>()).Returns(expectedResponse);

        // Act
        var result = await PermissionManagementApi.GetPermissionsV1(_sender);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetPermissions>());
    }

    [Fact]
    public async Task GetPermissionsV1_WithCustomParameters_ReturnsOkResult()
    {
        // Arrange
        var permissions = new[]
        {
            new Response.PermissionSummary(
                Guid.NewGuid(),
                "USER_MANAGEMENT",
                "READ",
                "Admin",
                "User Management",
                "Read")
        };

        var expectedResponse = Result.Success(new Response.PermissionList(
            permissions,
            1,
            1,
            5));

        _sender.Send(Arg.Any<Query.GetPermissions>()).Returns(expectedResponse);

        // Act
        var result = await PermissionManagementApi.GetPermissionsV1(_sender, page: 1, pageSize: 5);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetPermissions>());
    }

    [Fact]
    public async Task GetPermissionByIdV1_WithValidIds_ReturnsOkResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var functionId = "USER_MANAGEMENT";
        var actionId = "CREATE";

        var expectedResponse = Result.Success(new Response.PermissionDetails(
            roleId,
            functionId,
            actionId,
            "Admin",
            "User Management",
            "Create"));

        _sender.Send(Arg.Any<Query.GetPermissionById>()).Returns(expectedResponse);

        // Act
        var result = await PermissionManagementApi.GetPermissionByIdV1(_sender, roleId, functionId, actionId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetPermissionById>());
    }

    [Fact]
    public async Task GetPermissionByIdV1_WithInvalidIds_ReturnsNotFoundResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var functionId = "INVALID_FUNCTION";
        var actionId = "INVALID_ACTION";

        var errorResponse = Result.Failure<Response.PermissionDetails>(new Error("PERMISSION.NOT_FOUND", "Permission not found"));

        _sender.Send(Arg.Any<Query.GetPermissionById>()).Returns(errorResponse);

        // Act
        var result = await PermissionManagementApi.GetPermissionByIdV1(_sender, roleId, functionId, actionId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetPermissionById>());
    }

    [Fact]
    public async Task DeletePermissionV1_WithValidIds_ReturnsNoContentResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var functionId = "USER_MANAGEMENT";
        var actionId = "DELETE";

        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.DeletePermission>()).Returns(expectedResponse);

        // Act
        var result = await PermissionManagementApi.DeletePermissionV1(_sender, roleId, functionId, actionId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.DeletePermission>());
    }

    [Fact]
    public async Task DeletePermissionV1_WithInvalidIds_ReturnsFailureResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var functionId = "INVALID_FUNCTION";
        var actionId = "INVALID_ACTION";

        var errorResponse = Result.Failure(new Error("PERMISSION.NOT_FOUND", "Permission not found"));
        _sender.Send(Arg.Any<Command.DeletePermission>()).Returns(errorResponse);

        // Act
        var result = await PermissionManagementApi.DeletePermissionV1(_sender, roleId, functionId, actionId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.DeletePermission>());
    }

    [Fact]
    public async Task CreatePermissionV1_WithNullCommand_HandlesGracefully()
    {
        // Arrange
        Command.CreatePermission nullCommand = null!;
        var errorResponse = Result.Failure<Response.PermissionCreated>(new Error("PERMISSION.INVALID_REQUEST", "Invalid request"));

        _sender.Send(Arg.Any<Command.CreatePermission>()).Returns(errorResponse);

        // Act
        var result = await PermissionManagementApi.CreatePermissionV1(_sender, nullCommand);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(nullCommand);
    }

    [Fact]
    public async Task GetPermissionsV1_WithNegativePageParameters_HandlesGracefully()
    {
        // Arrange
        var expectedResponse = Result.Success(new Response.PermissionList(
            new List<PermissionSummary>(),
            0,
            1,
            10));

        _sender.Send(Arg.Any<Query.GetPermissions>()).Returns(expectedResponse);

        // Act
        var result = await PermissionManagementApi.GetPermissionsV1(_sender, page: -1, pageSize: -10);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetPermissions>());
    }
}
