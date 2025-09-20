namespace DemoCICD.UnitTests.Domain.Entities.MotoGP.RaceManagement;

public class RaceEntryTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldCreateRaceEntry()
    {
        // Arrange
        var raceId = Guid.NewGuid();
        var riderId = Guid.NewGuid();
        var teamId = Guid.NewGuid();
        var bikeId = Guid.NewGuid();
        const int startingGrid = 5;
        const string notes = "Test notes";

        // Act
        var raceEntry = RaceEntry.Create(raceId, riderId, teamId, bikeId, startingGrid, notes);

        // Assert
        raceEntry.Should().NotBeNull();
        raceEntry.Id.Should().NotBe(Guid.Empty);
        raceEntry.RaceId.Should().Be(raceId);
        raceEntry.RiderId.Should().Be(riderId);
        raceEntry.TeamId.Should().Be(teamId);
        raceEntry.BikeId.Should().Be(bikeId);
        raceEntry.StartingGrid.Should().Be(startingGrid);
        raceEntry.Notes.Should().Be(notes);
        raceEntry.FastestLap.Should().BeFalse();
        raceEntry.FastestLapTime.Should().BeNull();
        raceEntry.Result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5)]
    public void Create_WithInvalidStartingGrid_ShouldThrowArgumentException(int invalidStartingGrid)
    {
        // Arrange
        var raceId = Guid.NewGuid();
        var riderId = Guid.NewGuid();
        var teamId = Guid.NewGuid();
        var bikeId = Guid.NewGuid();

        // Act & Assert
        var act = () => RaceEntry.Create(raceId, riderId, teamId, bikeId, invalidStartingGrid);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Starting grid position must be positive*")
           .And.ParamName.Should().Be("startingGrid");
    }

    [Fact]
    public void UpdateStartingGrid_WithValidPosition_ShouldUpdatePosition()
    {
        // Arrange
        var raceEntry = CreateValidRaceEntry();
        const int newStartingGrid = 10;

        // Act
        raceEntry.UpdateStartingGrid(newStartingGrid);

        // Assert
        raceEntry.StartingGrid.Should().Be(newStartingGrid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void UpdateStartingGrid_WithInvalidPosition_ShouldThrowArgumentException(int invalidPosition)
    {
        // Arrange
        var raceEntry = CreateValidRaceEntry();

        // Act & Assert
        var act = () => raceEntry.UpdateStartingGrid(invalidPosition);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Starting grid position must be positive*")
           .And.ParamName.Should().Be("startingGrid");
    }

    [Fact]
    public void SetRaceResult_WithValidResult_ShouldSetResult()
    {
        // Arrange
        var raceEntry = CreateValidRaceEntry();
        var raceResult = new RaceResult(1, 25, TimeSpan.FromMinutes(45));

        // Act
        raceEntry.SetRaceResult(raceResult);

        // Assert
        raceEntry.Result.Should().Be(raceResult);
        raceEntry.IsFinisher.Should().BeTrue();
        raceEntry.Points.Should().Be(25);
    }

    [Fact]
    public void SetRaceResult_WithNullResult_ShouldThrowArgumentNullException()
    {
        // Arrange
        var raceEntry = CreateValidRaceEntry();

        // Act & Assert
        var act = () => raceEntry.SetRaceResult(null!);
        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be("result");
    }

    [Fact]
    public void SetFastestLap_WithValidTime_ShouldSetFastestLap()
    {
        // Arrange
        var raceEntry = CreateValidRaceEntry();
        var lapTime = TimeSpan.FromMinutes(1).Add(TimeSpan.FromSeconds(30));

        // Act
        raceEntry.SetFastestLap(lapTime);

        // Assert
        raceEntry.FastestLap.Should().BeTrue();
        raceEntry.FastestLapTime.Should().Be(lapTime);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void SetFastestLap_WithInvalidTime_ShouldThrowArgumentException(int seconds)
    {
        // Arrange
        var raceEntry = CreateValidRaceEntry();
        var invalidTime = TimeSpan.FromSeconds(seconds);

        // Act & Assert
        var act = () => raceEntry.SetFastestLap(invalidTime);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Lap time must be positive*")
           .And.ParamName.Should().Be("lapTime");
    }

    [Fact]
    public void RemoveFastestLap_ShouldClearFastestLapData()
    {
        // Arrange
        var raceEntry = CreateValidRaceEntry();
        raceEntry.SetFastestLap(TimeSpan.FromMinutes(1).Add(TimeSpan.FromSeconds(30)));

        // Act
        raceEntry.RemoveFastestLap();

        // Assert
        raceEntry.FastestLap.Should().BeFalse();
        raceEntry.FastestLapTime.Should().BeNull();
    }

    [Fact]
    public void UpdateNotes_WithValidNotes_ShouldUpdateNotes()
    {
        // Arrange
        var raceEntry = CreateValidRaceEntry();
        const string newNotes = "Updated notes";

        // Act
        raceEntry.UpdateNotes(newNotes);

        // Assert
        raceEntry.Notes.Should().Be(newNotes);
    }

    [Fact]
    public void UpdateNotes_WithNullNotes_ShouldSetNotesToNull()
    {
        // Arrange
        var raceEntry = CreateValidRaceEntry();

        // Act
        raceEntry.UpdateNotes(null);

        // Assert
        raceEntry.Notes.Should().BeNull();
    }

    private static RaceEntry CreateValidRaceEntry()
    {
        return RaceEntry.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            5,
            "Test notes");
    }
}