using DemoCICD.Domain.Services.MotoGP;

namespace DemoCICD.UnitTests.Domain.Services.MotoGP;

public class PointsCalculationServiceTests
{
    private readonly PointsCalculationService _service;

    public PointsCalculationServiceTests()
    {
        _service = new PointsCalculationService();
    }

    #region CalculateRacePoints Tests

    [Theory]
    [InlineData(1, 25)]
    [InlineData(2, 20)]
    [InlineData(3, 16)]
    [InlineData(4, 13)]
    [InlineData(5, 11)]
    [InlineData(6, 10)]
    [InlineData(7, 9)]
    [InlineData(8, 8)]
    [InlineData(9, 7)]
    [InlineData(10, 6)]
    [InlineData(11, 5)]
    [InlineData(12, 4)]
    [InlineData(13, 3)]
    [InlineData(14, 2)]
    [InlineData(15, 1)]
    public void CalculateRacePoints_WithValidPositions_ShouldReturnCorrectPoints(int position, int expectedPoints)
    {
        // Act
        var result = _service.CalculateRacePoints(position);

        // Assert
        result.Should().Be(expectedPoints);
    }

    [Theory]
    [InlineData(16)]
    [InlineData(20)]
    [InlineData(100)]
    public void CalculateRacePoints_WithPositionOutsidePointsRange_ShouldReturnZero(int position)
    {
        // Act
        var result = _service.CalculateRacePoints(position);

        // Assert
        result.Should().Be(0);
    }

    [Theory]
    [InlineData(1, true, false, false)]
    [InlineData(1, false, true, false)]
    [InlineData(1, false, false, true)]
    [InlineData(1, true, true, false)]
    [InlineData(1, true, false, true)]
    [InlineData(1, false, true, true)]
    [InlineData(1, true, true, true)]
    public void CalculateRacePoints_WithDnfDnsOrDisqualified_ShouldReturnZero(int position, bool didNotFinish, bool didNotStart, bool disqualified)
    {
        // Act
        var result = _service.CalculateRacePoints(position, didNotFinish, didNotStart, disqualified);

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region CalculateSprintPoints Tests

    [Theory]
    [InlineData(1, 12)]
    [InlineData(2, 9)]
    [InlineData(3, 7)]
    [InlineData(4, 6)]
    [InlineData(5, 5)]
    [InlineData(6, 4)]
    [InlineData(7, 3)]
    [InlineData(8, 2)]
    [InlineData(9, 1)]
    public void CalculateSprintPoints_WithValidPositions_ShouldReturnCorrectPoints(int position, int expectedPoints)
    {
        // Act
        var result = _service.CalculateSprintPoints(position);

        // Assert
        result.Should().Be(expectedPoints);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(100)]
    public void CalculateSprintPoints_WithPositionOutsidePointsRange_ShouldReturnZero(int position)
    {
        // Act
        var result = _service.CalculateSprintPoints(position);

        // Assert
        result.Should().Be(0);
    }

    [Theory]
    [InlineData(1, true, false, false)]
    [InlineData(1, false, true, false)]
    [InlineData(1, false, false, true)]
    [InlineData(1, true, true, false)]
    [InlineData(1, true, false, true)]
    [InlineData(1, false, true, true)]
    [InlineData(1, true, true, true)]
    public void CalculateSprintPoints_WithDnfDnsOrDisqualified_ShouldReturnZero(int position, bool didNotFinish, bool didNotStart, bool disqualified)
    {
        // Act
        var result = _service.CalculateSprintPoints(position, didNotFinish, didNotStart, disqualified);

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region CalculateRiderStats Tests

    [Fact]
    public void CalculateRiderStats_WithEmptyRaceResults_ShouldReturnZeroStats()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var raceResults = new List<RaceResult>();

        // Act
        var result = _service.CalculateRiderStats(riderId, seasonId, raceResults);

        // Assert
        result.RiderId.Should().Be(riderId);
        result.SeasonId.Should().Be(seasonId);
        result.TotalPoints.Should().Be(0);
        result.Wins.Should().Be(0);
        result.Podiums.Should().Be(0);
        result.RacesParticipated.Should().Be(0);
        result.RacesFinished.Should().Be(0);
        result.DidNotFinish.Should().Be(0);
        result.DidNotStart.Should().Be(0);
    }

    [Fact]
    public void CalculateRiderStats_WithSuccessfulRaces_ShouldCalculateCorrectStats()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var raceResults = new List<RaceResult>
        {
            new(1, 25), // Win
            new(2, 20), // Podium
            new(3, 16), // Podium
            new(5, 11), // Points finish
            new(10, 6)  // Points finish
        };

        // Act
        var result = _service.CalculateRiderStats(riderId, seasonId, raceResults);

        // Assert
        result.RiderId.Should().Be(riderId);
        result.SeasonId.Should().Be(seasonId);
        result.TotalPoints.Should().Be(78); // 25 + 20 + 16 + 11 + 6
        result.Wins.Should().Be(1);
        result.Podiums.Should().Be(3); // Positions 1, 2, 3
        result.RacesParticipated.Should().Be(5);
        result.RacesFinished.Should().Be(5);
        result.DidNotFinish.Should().Be(0);
        result.DidNotStart.Should().Be(0);
    }

    [Fact]
    public void CalculateRiderStats_WithMixedResults_ShouldCalculateCorrectStats()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var raceResults = new List<RaceResult>
        {
            new(1, 25), // Win
            new(0, 0, didNotFinish: true), // DNF
            new(0, 0, didNotStart: true), // DNS
            new(2, 20), // Podium
            new(0, 0, disqualified: true) // DSQ
        };

        // Act
        var result = _service.CalculateRiderStats(riderId, seasonId, raceResults);

        // Assert
        result.RiderId.Should().Be(riderId);
        result.SeasonId.Should().Be(seasonId);
        result.TotalPoints.Should().Be(45); // 25 + 20
        result.Wins.Should().Be(1);
        result.Podiums.Should().Be(2); // Positions 1, 2
        result.RacesParticipated.Should().Be(5);
        result.RacesFinished.Should().Be(2); // Only finished races count
        result.DidNotFinish.Should().Be(1);
        result.DidNotStart.Should().Be(1);
    }

    #endregion

    #region CalculateTeamStats Tests

    [Fact]
    public void CalculateTeamStats_WithEmptyRiderStats_ShouldReturnZeroStats()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var riderStats = new List<RiderStats>();

        // Act
        var result = _service.CalculateTeamStats(teamId, seasonId, riderStats);

        // Assert
        result.TeamId.Should().Be(teamId);
        result.SeasonId.Should().Be(seasonId);
        result.TotalPoints.Should().Be(0);
        result.Wins.Should().Be(0);
        result.Podiums.Should().Be(0);
        result.PolePositions.Should().Be(0);
        result.FastestLaps.Should().Be(0);
        result.RacesParticipated.Should().Be(0);
    }

    [Fact]
    public void CalculateTeamStats_WithSingleRider_ShouldSumStatsCorrectly()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var riderId = Guid.NewGuid();
        var riderStats = new List<RiderStats>
        {
            new(riderId, seasonId, 100, 3, 5, 2, 1, 10, 8, 1, 1)
        };

        // Act
        var result = _service.CalculateTeamStats(teamId, seasonId, riderStats);

        // Assert
        result.TeamId.Should().Be(teamId);
        result.SeasonId.Should().Be(seasonId);
        result.TotalPoints.Should().Be(100);
        result.Wins.Should().Be(3);
        result.Podiums.Should().Be(5);
        result.PolePositions.Should().Be(2);
        result.FastestLaps.Should().Be(1);
        result.RacesParticipated.Should().Be(10);
    }

    [Fact]
    public void CalculateTeamStats_WithMultipleRiders_ShouldSumStatsCorrectly()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var rider1Id = Guid.NewGuid();
        var rider2Id = Guid.NewGuid();
        var riderStats = new List<RiderStats>
        {
            new(rider1Id, seasonId, 150, 4, 7, 3, 2, 12, 10, 2, 0),
            new(rider2Id, seasonId, 80, 1, 3, 1, 1, 12, 9, 3, 0)
        };

        // Act
        var result = _service.CalculateTeamStats(teamId, seasonId, riderStats);

        // Assert
        result.TeamId.Should().Be(teamId);
        result.SeasonId.Should().Be(seasonId);
        result.TotalPoints.Should().Be(230); // 150 + 80
        result.Wins.Should().Be(5); // 4 + 1
        result.Podiums.Should().Be(10); // 7 + 3
        result.PolePositions.Should().Be(4); // 3 + 1
        result.FastestLaps.Should().Be(3); // 2 + 1
        result.RacesParticipated.Should().Be(12); // Max of 12 and 12
    }

    [Fact]
    public void CalculateTeamStats_WithDifferentRacesParticipated_ShouldUseMaximum()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var rider1Id = Guid.NewGuid();
        var rider2Id = Guid.NewGuid();
        var riderStats = new List<RiderStats>
        {
            new(rider1Id, seasonId, 100, 2, 4, 1, 1, 15, 12, 3, 0), // 15 races
            new(rider2Id, seasonId, 50, 1, 2, 0, 0, 8, 6, 2, 0)    // 8 races
        };

        // Act
        var result = _service.CalculateTeamStats(teamId, seasonId, riderStats);

        // Assert
        result.RacesParticipated.Should().Be(15); // Max of 15 and 8
        result.TotalPoints.Should().Be(150); // 100 + 50
        result.Wins.Should().Be(3); // 2 + 1
    }

    #endregion
}