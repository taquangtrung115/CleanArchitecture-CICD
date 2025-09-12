using System.Threading;
using System.Threading.Tasks;
using DemoCICD.Application.Abstractions;
using DemoCICD.Application.UserCases.V1.Commands.Identity;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DemoCICD.Tests.Application.Identity;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IUserAuthenticationService> _mockUserAuthService;
    private readonly Mock<ILogger<RegisterCommandHandler>> _mockLogger;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _mockUserAuthService = new Mock<IUserAuthenticationService>();
        _mockLogger = new Mock<ILogger<RegisterCommandHandler>>();
        _handler = new RegisterCommandHandler(_mockUserAuthService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_WhenRegistrationSucceeds_ShouldReturnSuccess()
    {
        // Arrange
        var command = new Command.Register(
            "testuser",
            "test@example.com",
            "password123",
            "Test",
            "User",
            null
        );

        var authResult = UserAuthResult.Success("123", "testuser", "test@example.com", "Test User");
        _mockUserAuthService.Setup(x => x.RegisterUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<DateTime?>()))
            .ReturnsAsync(authResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("testuser", result.Value.UserName);
        Assert.Equal("test@example.com", result.Value.Email);
    }

    [Fact]
    public async Task Handle_WhenRegistrationFails_ShouldReturnFailure()
    {
        // Arrange
        var command = new Command.Register(
            "testuser",
            "test@example.com",
            "password123",
            "Test",
            "User",
            null
        );

        var authResult = UserAuthResult.Failure("Registration failed");
        _mockUserAuthService.Setup(x => x.RegisterUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<DateTime?>()))
            .ReturnsAsync(authResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Registration failed", result.Error.Message);
    }
}