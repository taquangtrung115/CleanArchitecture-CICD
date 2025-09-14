using DemoCICD.Application.Abstractions;
using DemoCICD.Application.UserCases.V1.Commands.Identity;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace DemoCICD.UnitTests.Application.Commands.Identity;

public class CreateUserCommandHandlerTests
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userManagementService = Substitute.For<IUserManagementService>();
        _logger = Substitute.For<ILogger<CreateUserCommandHandler>>();
        _handler = new CreateUserCommandHandler(_userManagementService, _logger);
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
            Guid.NewGuid());

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
            command.ManagerId,
            command.PositionId)
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
            command.ManagerId,
            command.PositionId);
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
            Guid.NewGuid());

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
            command.ManagerId,
            command.PositionId)
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
            command.ManagerId,
            command.PositionId);
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
            Guid.NewGuid());

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

    [Fact]
    public async Task Handle_WithSuccessfulCreation_ShouldLogInformation()
    {
        // Arrange
        var command = new Command.CreateUser(
            "jane.smith",
            "jane.smith@example.com",
            "SecurePassword123",
            "Jane",
            "Smith",
            new DateTime(1985, 3, 20),
            true,
            false,
            null,
            Guid.NewGuid());

        var userId = Guid.NewGuid();
        var userAuthResult = UserAuthResult.Success(
            userId.ToString(),
            "jane.smith",
            "jane.smith@example.com",
            "Jane Smith");

        _userManagementService.CreateUserAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<DateTime?>(),
            Arg.Any<bool?>(),
            Arg.Any<bool?>(),
            Arg.Any<Guid?>(),
            Arg.Any<Guid>())
            .Returns(userAuthResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        _logger.Received(1).LogInformation(
            "User {UserName} created successfully with ID {UserId}",
            "jane.smith",
            userId.ToString());
    }

    [Fact]
    public async Task Handle_WithException_ShouldLogError()
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
            Guid.NewGuid());

        var exception = new InvalidOperationException("Database connection failed");

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
            .Do(x => throw exception);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _logger.Received(1).LogError(exception, "Error during user creation for user: {UserName}", "john.doe");
    }

    [Fact]
    public async Task Handle_WithNullErrorMessage_ShouldUseDefaultErrorMessage()
    {
        // Arrange
        var command = new Command.CreateUser(
            "test.user",
            "test@example.com",
            "password",
            "Test",
            "User",
            null,
            false,
            false,
            null,
            Guid.NewGuid());

        var userAuthResult = new UserAuthResult
        {
            IsSuccess = false,
            ErrorMessage = null
        };

        _userManagementService.CreateUserAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<DateTime?>(),
            Arg.Any<bool?>(),
            Arg.Any<bool?>(),
            Arg.Any<Guid?>(),
            Arg.Any<Guid>())
            .Returns(userAuthResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("UserCreation.Failed");
        result.Error.Message.Should().Be("User creation failed");
    }
}