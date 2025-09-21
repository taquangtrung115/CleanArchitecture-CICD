using DemoCICD.Application.Abstractions;
using DemoCICD.Application.UserCases.V1.Commands.Identity;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace DemoCICD.Architecture.Application.Commands.Identity;

public class CreateUserCommandHandlerTests
{
    private readonly IUserManagementService _userManagementService;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userManagementService = Substitute.For<IUserManagementService>();
        _handler = new CreateUserCommandHandler(_userManagementService);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnSuccessResult()
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

        var userId = Guid.NewGuid();
        var userAuthResult = UserAuthResult.Success(
            userId.ToString(),
            "john.doe",
            "john.doe@example.com",
            "John Doe");

        _userManagementService.CreateUserAsync(
            command.UserName,
            command.Email,
            command.Password,
            command.FirstName,
            command.LastName,
            command.DayOfBirth,
            command.IsDirector,
            command.IsHeadOfDepartment,
            Arg.Any<Guid?>(),
            Arg.Any<Guid>())
            .Returns(userAuthResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.UserId.Should().Be(userId);
        result.Value.UserName.Should().Be("john.doe");
        result.Value.Email.Should().Be("john.doe@example.com");

        await _userManagementService.Received(1).CreateUserAsync(
            command.UserName,
            command.Email,
            command.Password,
            command.FirstName,
            command.LastName,
            command.DayOfBirth,
            command.IsDirector,
            command.IsHeadOfDepartment,
            Arg.Any<Guid?>(),
            Arg.Any<Guid>());
    }

    [Fact]
    public async Task Handle_WithFailedUserCreation_ShouldReturnFailureResult()
    {
        // Arrange
        var command = new Command.CreateUser(
            "invalid.user",
            "invalid-email",
            "weak",
            "",
            "",
            null,
            false,
            false,
            null,
            Guid.NewGuid().ToString());

        var userAuthResult = UserAuthResult.Failure("Email already exists");

        _userManagementService.CreateUserAsync(
            command.UserName,
            command.Email,
            command.Password,
            command.FirstName,
            command.LastName,
            command.DayOfBirth,
            command.IsDirector,
            command.IsHeadOfDepartment,
            Arg.Any<Guid?>(),
            Arg.Any<Guid>())
            .Returns(userAuthResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("UserCreation.Failed");
        result.Error.Message.Should().Be("Email already exists");

        await _userManagementService.Received(1).CreateUserAsync(
            command.UserName,
            command.Email,
            command.Password,
            command.FirstName,
            command.LastName,
            command.DayOfBirth,
            command.IsDirector,
            command.IsHeadOfDepartment,
            Arg.Any<Guid?>(),
            Arg.Any<Guid>());
    }

    [Fact]
    public async Task Handle_WithExceptionThrown_ShouldReturnFailureResult()
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

        _userManagementService.When(x => x.CreateUserAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<DateTime?>(),
            Arg.Any<bool?>(),
            Arg.Any<bool?>(),
            Arg.Any<Guid?>(),
            Arg.Any<Guid>()))
            .Do(x => throw new InvalidOperationException("Database connection failed"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("UserCreation.Error");
        result.Error.Message.Should().Be("An error occurred during user creation");
    }
}
