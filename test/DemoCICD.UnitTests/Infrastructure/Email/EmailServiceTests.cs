using DemoCICD.Infrastructure.Email;

namespace DemoCICD.UnitTests.Infrastructure.Email;

public class EmailServiceTests
{
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        _emailService = new EmailService();
    }

    #region SendPasswordResetEmailAsync Tests

    [Fact]
    public async Task SendPasswordResetEmailAsync_WithValidParameters_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test@example.com";
        var resetToken = "reset-token-123";
        var userName = "TestUser";

        // Act
        var result = await _emailService.SendPasswordResetEmailAsync(toEmail, resetToken, userName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendPasswordResetEmailAsync_WithEmptyEmail_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "";
        var resetToken = "reset-token-123";
        var userName = "TestUser";

        // Act
        var result = await _emailService.SendPasswordResetEmailAsync(toEmail, resetToken, userName);

        // Assert
        result.Should().BeTrue(); // Current implementation always returns true for demo purposes
    }

    [Fact]
    public async Task SendPasswordResetEmailAsync_WithNullEmail_ShouldReturnTrue()
    {
        // Arrange
        string? toEmail = null;
        var resetToken = "reset-token-123";
        var userName = "TestUser";

        // Act
        var result = await _emailService.SendPasswordResetEmailAsync(toEmail!, resetToken, userName);

        // Assert
        result.Should().BeTrue(); // Current implementation always returns true for demo purposes
    }

    [Fact]
    public async Task SendPasswordResetEmailAsync_WithEmptyResetToken_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test@example.com";
        var resetToken = "";
        var userName = "TestUser";

        // Act
        var result = await _emailService.SendPasswordResetEmailAsync(toEmail, resetToken, userName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendPasswordResetEmailAsync_WithNullResetToken_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test@example.com";
        string? resetToken = null;
        var userName = "TestUser";

        // Act
        var result = await _emailService.SendPasswordResetEmailAsync(toEmail, resetToken!, userName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendPasswordResetEmailAsync_WithEmptyUserName_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test@example.com";
        var resetToken = "reset-token-123";
        var userName = "";

        // Act
        var result = await _emailService.SendPasswordResetEmailAsync(toEmail, resetToken, userName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendPasswordResetEmailAsync_WithNullUserName_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test@example.com";
        var resetToken = "reset-token-123";
        string? userName = null;

        // Act
        var result = await _emailService.SendPasswordResetEmailAsync(toEmail, resetToken, userName!);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region SendEmailAsync Tests

    [Fact]
    public async Task SendEmailAsync_WithValidParameters_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test@example.com";
        var subject = "Test Subject";
        var body = "Test email body content";

        // Act
        var result = await _emailService.SendEmailAsync(toEmail, subject, body);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendEmailAsync_WithEmptyEmail_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "";
        var subject = "Test Subject";
        var body = "Test email body content";

        // Act
        var result = await _emailService.SendEmailAsync(toEmail, subject, body);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendEmailAsync_WithNullEmail_ShouldReturnTrue()
    {
        // Arrange
        string? toEmail = null;
        var subject = "Test Subject";
        var body = "Test email body content";

        // Act
        var result = await _emailService.SendEmailAsync(toEmail!, subject, body);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendEmailAsync_WithEmptySubject_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test@example.com";
        var subject = "";
        var body = "Test email body content";

        // Act
        var result = await _emailService.SendEmailAsync(toEmail, subject, body);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendEmailAsync_WithNullSubject_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test@example.com";
        string? subject = null;
        var body = "Test email body content";

        // Act
        var result = await _emailService.SendEmailAsync(toEmail, subject!, body);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendEmailAsync_WithEmptyBody_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test@example.com";
        var subject = "Test Subject";
        var body = "";

        // Act
        var result = await _emailService.SendEmailAsync(toEmail, subject, body);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendEmailAsync_WithNullBody_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test@example.com";
        var subject = "Test Subject";
        string? body = null;

        // Act
        var result = await _emailService.SendEmailAsync(toEmail, subject, body!);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendEmailAsync_WithLongContent_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test@example.com";
        var subject = "Very long subject that might test email service limits and edge cases";
        var body = string.Join("\n", Enumerable.Repeat("This is a very long email body line that tests the service with substantial content.", 100));

        // Act
        var result = await _emailService.SendEmailAsync(toEmail, subject, body);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendEmailAsync_WithSpecialCharacters_ShouldReturnTrue()
    {
        // Arrange
        var toEmail = "test+special@example.com";
        var subject = "Subject with special chars: éñ中文🚀";
        var body = "Body with special characters: <script>alert('test')</script> & entities";

        // Act
        var result = await _emailService.SendEmailAsync(toEmail, subject, body);

        // Assert
        result.Should().BeTrue();
    }

    #endregion
}