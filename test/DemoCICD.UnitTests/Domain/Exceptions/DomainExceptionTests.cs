using DemoCICD.Domain.Exceptions;

namespace DemoCICD.UnitTests.Domain.Exceptions;

// Create a concrete implementation for testing the abstract class
public class TestDomainException : DomainException
{
    public TestDomainException(string title, string message) : base(title, message)
    {
    }
}

public class TestNotFoundException : NotFoundException
{
    public TestNotFoundException(string message) : base(message)
    {
    }
}

public class DomainExceptionTests
{
    [Fact]
    public void DomainException_WithTitleAndMessage_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const string title = "Test Title";
        const string message = "Test message for domain exception";

        // Act
        var exception = new TestDomainException(title, message);

        // Assert
        exception.Title.Should().Be(title);
        exception.Message.Should().Be(message);
        exception.Should().BeAssignableTo<Exception>();
        exception.Should().BeAssignableTo<DomainException>();
    }

    [Fact]
    public void DomainException_WithNullTitle_ShouldStillWork()
    {
        // Arrange
        const string? title = null;
        const string message = "Test message";

        // Act
        var exception = new TestDomainException(title!, message);

        // Assert
        exception.Title.Should().BeNull();
        exception.Message.Should().Be(message);
    }

    [Fact]
    public void NotFoundException_WithMessage_ShouldSetTitleToNotFound()
    {
        // Arrange
        const string message = "Entity not found";

        // Act
        var exception = new TestNotFoundException(message);

        // Assert
        exception.Title.Should().Be("Not Found");
        exception.Message.Should().Be(message);
        exception.Should().BeAssignableTo<DomainException>();
        exception.Should().BeAssignableTo<NotFoundException>();
    }

    [Fact]
    public void NotFoundException_WithEmptyMessage_ShouldStillWork()
    {
        // Arrange
        const string message = "";

        // Act
        var exception = new TestNotFoundException(message);

        // Assert
        exception.Title.Should().Be("Not Found");
        exception.Message.Should().Be(message);
    }

    [Fact]
    public void DomainException_WhenThrown_ShouldBeCatchableAsException()
    {
        // Arrange
        const string title = "Error";
        const string message = "Something went wrong";

        // Act & Assert
        Action act = () => throw new TestDomainException(title, message);
        act.Should().Throw<Exception>()
           .Which.Message.Should().Be(message);
    }

    [Fact]
    public void NotFoundException_WhenThrown_ShouldBeCatchableAsDomainException()
    {
        // Arrange
        const string message = "Resource not found";

        // Act & Assert
        Action act = () => throw new TestNotFoundException(message);
        act.Should().Throw<DomainException>()
           .Which.Title.Should().Be("Not Found");
    }
}