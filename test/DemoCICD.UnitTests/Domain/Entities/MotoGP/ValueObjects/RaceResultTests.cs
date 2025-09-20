namespace DemoCICD.UnitTests.Domain.Entities.MotoGP.ValueObjects;

public class RaceResultTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateRaceResult()
    {
        // Arrange
        const int position = 1;
        const int points = 25;
        var raceTime = TimeSpan.FromMinutes(45);
        var gapToWinner = TimeSpan.Zero;

        // Act
        var raceResult = new RaceResult(position, points, raceTime, gapToWinner);

        // Assert
        raceResult.Position.Should().Be(position);
        raceResult.Points.Should().Be(points);
        raceResult.RaceTime.Should().Be(raceTime);
        raceResult.GapToWinner.Should().Be(gapToWinner);
        raceResult.DidNotFinish.Should().BeFalse();
        raceResult.DidNotStart.Should().BeFalse();
        raceResult.Disqualified.Should().BeFalse();
        raceResult.IsFinisher.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithInvalidPositionAndFinishedRace_ShouldThrowArgumentException(int invalidPosition)
    {
        // Act & Assert
        var act = () => new RaceResult(invalidPosition, 10);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Position must be greater than 0 for finished races*")
           .And.ParamName.Should().Be("position");
    }

    [Fact]
    public void Constructor_WithNegativePoints_ShouldThrowArgumentException()
    {
        // Act & Assert
        var act = () => new RaceResult(1, -5);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Points cannot be negative*")
           .And.ParamName.Should().Be("points");
    }

    [Fact]
    public void Constructor_WithDidNotFinish_ShouldAllowZeroPosition()
    {
        // Act
        var raceResult = new RaceResult(0, 0, didNotFinish: true, reason: "Mechanical failure");

        // Assert
        raceResult.Position.Should().Be(0);
        raceResult.Points.Should().Be(0);
        raceResult.DidNotFinish.Should().BeTrue();
        raceResult.IsFinisher.Should().BeFalse();
        raceResult.Reason.Should().Be("Mechanical failure");
    }

    [Fact]
    public void Constructor_WithDidNotStart_ShouldAllowZeroPosition()
    {
        // Act
        var raceResult = new RaceResult(0, 0, didNotStart: true, reason: "Bike issue");

        // Assert
        raceResult.Position.Should().Be(0);
        raceResult.Points.Should().Be(0);
        raceResult.DidNotStart.Should().BeTrue();
        raceResult.IsFinisher.Should().BeFalse();
        raceResult.Reason.Should().Be("Bike issue");
    }

    [Fact]
    public void Constructor_WithDisqualified_ShouldAllowZeroPosition()
    {
        // Act
        var raceResult = new RaceResult(0, 0, disqualified: true, reason: "Technical infringement");

        // Assert
        raceResult.Position.Should().Be(0);
        raceResult.Points.Should().Be(0);
        raceResult.Disqualified.Should().BeTrue();
        raceResult.IsFinisher.Should().BeFalse();
        raceResult.Reason.Should().Be("Technical infringement");
    }

    [Fact]
    public void IsFinisher_WithSuccessfulRace_ShouldReturnTrue()
    {
        // Arrange
        var raceResult = new RaceResult(5, 10);

        // Act & Assert
        raceResult.IsFinisher.Should().BeTrue();
    }

    [Theory]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, true)]
    public void IsFinisher_WithUnsuccessfulRace_ShouldReturnFalse(bool dnf, bool dns, bool dsq)
    {
        // Arrange
        var raceResult = new RaceResult(0, 0, didNotFinish: dnf, didNotStart: dns, disqualified: dsq);

        // Act & Assert
        raceResult.IsFinisher.Should().BeFalse();
    }

    [Fact]
    public void ToString_WithSuccessfulFinish_ShouldReturnFormattedString()
    {
        // Arrange
        var raceResult = new RaceResult(3, 15);

        // Act
        var result = raceResult.ToString();

        // Assert
        result.Should().Be("P3 (15 pts)");
    }

    [Fact]
    public void ToString_WithDidNotStart_ShouldReturnDNS()
    {
        // Arrange
        var raceResult = new RaceResult(0, 0, didNotStart: true);

        // Act
        var result = raceResult.ToString();

        // Assert
        result.Should().Be("DNS");
    }

    [Fact]
    public void ToString_WithDidNotFinish_ShouldReturnDNF()
    {
        // Arrange
        var raceResult = new RaceResult(0, 0, didNotFinish: true);

        // Act
        var result = raceResult.ToString();

        // Assert
        result.Should().Be("DNF");
    }

    [Fact]
    public void ToString_WithDisqualified_ShouldReturnDSQ()
    {
        // Arrange
        var raceResult = new RaceResult(0, 0, disqualified: true);

        // Act
        var result = raceResult.ToString();

        // Assert
        result.Should().Be("DSQ");
    }

    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var raceTime = TimeSpan.FromMinutes(45);
        var result1 = new RaceResult(1, 25, raceTime);
        var result2 = new RaceResult(1, 25, raceTime);

        // Act & Assert
        result1.Equals(result2).Should().BeTrue();
        result1.GetHashCode().Should().Be(result2.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var result1 = new RaceResult(1, 25, TimeSpan.FromMinutes(45));
        var result2 = new RaceResult(2, 18, TimeSpan.FromMinutes(46));

        // Act & Assert
        result1.Equals(result2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var result = new RaceResult(1, 25);

        // Act & Assert
        result.Equals(null).Should().BeFalse();
    }
}