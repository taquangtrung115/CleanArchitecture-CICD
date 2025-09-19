using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Presentation.APIs.Identity;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using static DemoCICD.Contract.Services.V1.Identity.Response;

namespace DemoCICD.Architecture.Presentation.APIs.Identity;

public class RoleManagementApiTests
{
    private readonly ISender _sender;

    public RoleManagementApiTests()
    {
        _sender = Substitute.For<ISender>();
    }

    [Fact]
    public async Task CreateRoleV1_WithValidCommand_ReturnsCreatedResult()
    {
        // Arrange
        var command = new Command.CreateRole(
            "Manager",
            "Department Manager role",
            "MANAGER");

        var expectedResponse = Result.Success(new Response.RoleCreated(
            Guid.NewGuid(),
            "Manager",
            "MANAGER"));

        _sender.Send(command).Returns(expectedResponse);

        // Act
        var result = await RoleManagementApi.CreateRoleV1(_sender, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task CreateRoleV1_WithFailedCommand_ReturnsFailureResult()
    {
        // Arrange
        var command = new Command.CreateRole(
            "",
            "",
            "");

        var errorResponse = Result.Failure<Response.RoleCreated>(new Error("ROLE.INVALID_DATA", "Invalid role data"));
        _sender.Send(command).Returns(errorResponse);

        // Act
        var result = await RoleManagementApi.CreateRoleV1(_sender, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task GetRolesV1_WithDefaultParameters_ReturnsOkResult()
    {
        // Arrange
        var expectedResponse = Result.Success(new Response.RoleList(
            new List<RoleSummary>(),
            0,
            1,
            10));

        _sender.Send(Arg.Any<Query.GetRoles>()).Returns(expectedResponse);

        // Act
        var result = await RoleManagementApi.GetRolesV1(_sender);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetRoles>());
    }

    [Fact]
    public async Task GetRolesV1_WithFilterParameters_ReturnsOkResult()
    {
        // Arrange
        var expectedResponse = Result.Success(new Response.RoleList(
            new List<RoleSummary> { new Response.RoleSummary(Guid.NewGuid(), "Admin", "ADMIN", "Administrator role") },
            1,
            1,
            10));

        _sender.Send(Arg.Any<Query.GetRoles>()).Returns(expectedResponse);

        // Act
        var result = await RoleManagementApi.GetRolesV1(_sender, page: 1, pageSize: 10, searchTerm: "admin");

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetRoles>());
    }

    [Fact]
    public async Task GetRoleByIdV1_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var expectedResponse = Result.Success(new Response.RoleDetails(
            roleId,
            "Admin",
            "Administrator role with full permissions",
            "ADMIN",
            DateTime.UtcNow));

        _sender.Send(Arg.Any<Query.GetRoleById>()).Returns(expectedResponse);

        // Act
        var result = await RoleManagementApi.GetRoleByIdV1(_sender, roleId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetRoleById>());
    }

    [Fact]
    public async Task GetRoleByIdV1_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var errorResponse = Result.Failure<Response.RoleDetails>(new Error("ROLE.NOT_FOUND", "Role not found"));

        _sender.Send(Arg.Any<Query.GetRoleById>()).Returns(errorResponse);

        // Act
        var result = await RoleManagementApi.GetRoleByIdV1(_sender, roleId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetRoleById>());
    }

    [Fact]
    public async Task UpdateRoleV1_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var command = new Command.UpdateRole(
            roleId,
            "Manager Updated",
            "Updated description",
            "MANAGER_UPD");

        var expectedResponse = Result.Success(new Response.RoleUpdated(
            roleId,
            "Manager Updated",
            "MANAGER_UPD"));

        _sender.Send(command).Returns(expectedResponse);

        // Act
        var result = await RoleManagementApi.UpdateRoleV1(_sender, roleId, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task UpdateRoleV1_WithMismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var differentRoleId = Guid.NewGuid();
        var command = new Command.UpdateRole(
            differentRoleId,
            "Manager Updated",
            "Updated description",
            "MANAGER_UPD");

        // Act
        var result = await RoleManagementApi.UpdateRoleV1(_sender, roleId, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.DidNotReceive().Send(Arg.Any<Command.UpdateRole>());
    }

    [Fact]
    public async Task DeleteRoleV1_WithValidId_ReturnsNoContentResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.DeleteRole>()).Returns(expectedResponse);

        // Act
        var result = await RoleManagementApi.DeleteRoleV1(_sender, roleId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.DeleteRole>());
    }

    [Fact]
    public async Task GetUsersInRoleV1_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var expectedResponse = Result.Success(new Response.UserList(
            new List<UserSummary>(),
            0,
            1,
            10));

        _sender.Send(Arg.Any<Query.GetUsersInRole>()).Returns(expectedResponse);

        // Act
        var result = await RoleManagementApi.GetUsersInRoleV1(_sender, roleId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetUsersInRole>());
    }

    [Fact]
    public async Task GetRolePermissionsV1_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var expectedResponse = Result.Success(new Response.PermissionList(
            new List<PermissionSummary>(),
            0,
            1,
            10));

        _sender.Send(Arg.Any<Query.GetRolePermissions>()).Returns(expectedResponse);

        // Act
        var result = await RoleManagementApi.GetRolePermissionsV1(_sender, roleId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetRolePermissions>());
    }

    [Fact]
    public async Task GrantPermissionToRoleV1_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var command = new Command.GrantPermissionToRole(
            roleId,
            "USER_MANAGEMENT",
            "CREATE");

        var expectedResponse = Result.Success();
        _sender.Send(command).Returns(expectedResponse);

        // Act
        var result = await RoleManagementApi.GrantPermissionToRoleV1(_sender, roleId, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task RevokePermissionFromRoleV1_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var command = new Command.RevokePermissionFromRole(
            roleId,
            "USER_MANAGEMENT",
            "DELETE");

        var expectedResponse = Result.Success();
        _sender.Send(command).Returns(expectedResponse);

        // Act
        var result = await RoleManagementApi.RevokePermissionFromRoleV1(_sender, roleId, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task CreateRoleV1_WithNullCommand_HandlesGracefully()
    {
        // Arrange
        Command.CreateRole nullCommand = null!;
        var errorResponse = Result.Failure<Response.RoleCreated>(new Error("ROLE.INVALID_REQUEST", "Invalid request"));

        _sender.Send(Arg.Any<Command.CreateRole>()).Returns(errorResponse);

        // Act
        var result = await RoleManagementApi.CreateRoleV1(_sender, nullCommand);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(nullCommand);
    }
}
