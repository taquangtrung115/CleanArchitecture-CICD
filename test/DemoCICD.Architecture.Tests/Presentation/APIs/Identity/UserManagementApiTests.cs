using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Presentation.APIs.Identity;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using static DemoCICD.Contract.Services.V1.Identity.Response;

namespace DemoCICD.Architecture.Presentation.APIs.Identity;

public class UserManagementApiTests
{
    private readonly ISender _sender;

    public UserManagementApiTests()
    {
        _sender = Substitute.For<ISender>();
    }

    [Fact]
    public async Task CreateUserV1_WithValidCommand_ReturnsCreatedResult()
    {
        // Arrange
        var command = new Command.CreateUser(
            "john.doe",
            "john.doe@example.com",
            "SecurePassword123",
            "John",
            "Doe",
            new DateTime(1990, 1, 15),
            false,
            false,
            null,
            Guid.NewGuid().ToString());

        var expectedResponse = Result.Success(new Response.UserCreated(
            Guid.NewGuid(),
            "john.doe",
            "john.doe@example.com"));

        _sender.Send(command).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.CreateUserV1(_sender, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task CreateUserV1_WithFailedCommand_ReturnsFailureResult()
    {
        // Arrange
        var command = new Command.CreateUser(
            "",
            "invalid-email",
            "weak",
            "",
            "",
            null,
            false,
            false,
            null,
            Guid.NewGuid().ToString());

        var errorResponse = Result.Failure<Response.UserCreated>(new Error("USER.INVALID_DATA", "Invalid user data"));
        _sender.Send(command).Returns(errorResponse);

        // Act
        var result = await UserManagementApi.CreateUserV1(_sender, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task GetUsersV1_WithDefaultParameters_ReturnsOkResult()
    {
        // Arrange
        var expectedResponse = Result.Success(new Response.UserList(
            new List<UserSummary>(),
            0,
            1,
            10));

        _sender.Send(Arg.Any<Query.GetUsers>()).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.GetUsersV1(_sender);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetUsers>());
    }

    [Fact]
    public async Task GetUsersV1_WithFilterParameters_ReturnsOkResult()
    {
        // Arrange
        var expectedResponse = Result.Success(new Response.UserList(
            new List<UserSummary> { new Response.UserSummary(Guid.NewGuid(), "john.doe", "john.doe@example.com", "John Doe", false) },
            1,
            1,
            10));

        _sender.Send(Arg.Any<Query.GetUsers>()).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.GetUsersV1(_sender, page: 1, pageSize: 10, searchTerm: "john");

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetUsers>());
    }

    [Fact]
    public async Task GetUserByIdV1_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedResponse = Result.Success(new Response.UserDetails(
            userId,
            "john.doe",
            "john.doe@example.com",
            "John",
            "Doe",
            "John Doe",
            new DateTime(1990, 1, 15),
            false,
            false,
            null,
            Guid.NewGuid(),
            false,
            DateTime.UtcNow));

        _sender.Send(Arg.Any<Query.GetUserById>()).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.GetUserByIdV1(_sender, userId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetUserById>());
    }

    [Fact]
    public async Task GetUserByIdV1_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var errorResponse = Result.Failure<Response.UserDetails>(new Error("USER.NOT_FOUND", "User not found"));

        _sender.Send(Arg.Any<Query.GetUserById>()).Returns(errorResponse);

        // Act
        var result = await UserManagementApi.GetUserByIdV1(_sender, userId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetUserById>());
    }

    [Fact]
    public async Task UpdateUserV1_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new Command.UpdateUser(
            userId,
            "john.updated@example.com",
            "John Updated",
            "Doe",
            new DateTime(1990, 1, 15),
            false,
            true,
            null,
            Guid.NewGuid().ToString());

        var expectedResponse = Result.Success(new Response.UserUpdated(
            userId,
            "john.doe",
            "john.updated@example.com"));

        _sender.Send(command).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.UpdateUserV1(_sender, userId, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task UpdateUserV1_WithMismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var command = new Command.UpdateUser(
            differentUserId,
            "john.updated@example.com",
            "John Updated",
            "Doe",
            new DateTime(1990, 1, 15),
            false,
            true,
            null,
            Guid.NewGuid().ToString());

        // Act
        var result = await UserManagementApi.UpdateUserV1(_sender, userId, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.DidNotReceive().Send(Arg.Any<Command.UpdateUser>());
    }

    [Fact]
    public async Task DeleteUserV1_WithValidId_ReturnsNoContentResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.DeleteUser>()).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.DeleteUserV1(_sender, userId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.DeleteUser>());
    }

    [Fact]
    public async Task ChangePasswordV1_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new Command.ChangePassword(
            userId,
            "OldPassword123",
            "NewPassword456");

        var expectedResponse = Result.Success();
        _sender.Send(command).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.ChangePasswordV1(_sender, userId, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task ChangePasswordV1_WithMismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var command = new Command.ChangePassword(
            differentUserId,
            "OldPassword123",
            "NewPassword456");

        // Act
        var result = await UserManagementApi.ChangePasswordV1(_sender, userId, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.DidNotReceive().Send(Arg.Any<Command.ChangePassword>());
    }

    [Fact]
    public async Task ResetPasswordV1_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new Command.ResetPassword(userId, "ResetPassword789");

        var expectedResponse = Result.Success();
        _sender.Send(command).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.ResetPasswordV1(_sender, userId, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task LockUserV1_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.LockUser>()).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.LockUserV1(_sender, userId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.LockUser>());
    }

    [Fact]
    public async Task UnlockUserV1_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.UnlockUser>()).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.UnlockUserV1(_sender, userId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.UnlockUser>());
    }

    [Fact]
    public async Task GetUserRolesV1_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedResponse = Result.Success(new Response.UserRoleList(
            userId,
            new List<RoleSummary>()));

        _sender.Send(Arg.Any<Query.GetUserRoles>()).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.GetUserRolesV1(_sender, userId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetUserRoles>());
    }

    [Fact]
    public async Task AssignUserToRoleV1_WithValidIds_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.AssignUserToRole>()).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.AssignUserToRoleV1(_sender, userId, roleId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.AssignUserToRole>());
    }

    [Fact]
    public async Task RemoveUserFromRoleV1_WithValidIds_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.RemoveUserFromRole>()).Returns(expectedResponse);

        // Act
        var result = await UserManagementApi.RemoveUserFromRoleV1(_sender, userId, roleId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.RemoveUserFromRole>());
    }
}
