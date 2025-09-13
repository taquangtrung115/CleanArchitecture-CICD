using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Contract.Services.V1.Identity.Validators;
using Xunit;

namespace DemoCICD.Tests.Application.Identity;

public class RegisterValidatorTests
{
    private readonly RegisterValidator _validator;

    public RegisterValidatorTests()
    {
        _validator = new RegisterValidator();
    }

    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
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

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WithEmptyUserName_ShouldFail()
    {
        // Arrange
        var command = new Command.Register(
            "",
            "test@example.com",
            "password123",
            "Test",
            "User",
            null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "UserName");
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldFail()
    {
        // Arrange
        var command = new Command.Register(
            "testuser",
            "invalid-email",
            "password123",
            "Test",
            "User",
            null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_WithShortPassword_ShouldFail()
    {
        // Arrange
        var command = new Command.Register(
            "testuser",
            "test@example.com",
            "123",
            "Test",
            "User",
            null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }
}