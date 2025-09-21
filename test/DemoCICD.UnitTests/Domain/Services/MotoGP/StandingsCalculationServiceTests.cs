using DemoCICD.Domain.Services.MotoGP;

namespace DemoCICD.UnitTests.Domain.Services.MotoGP;

public class StandingsCalculationServiceTests
{
    private readonly StandingsCalculationService _service;

    public StandingsCalculationServiceTests()
    {
        _service = new StandingsCalculationService();
    }

    #region CalculateRiderStandings Tests

    [Fact]
    public void CalculateRiderStandings_WithEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        var riderStats = new List<RiderStats>();

        // Act
        var result = _service.CalculateRiderStandings(riderStats);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void CalculateRiderStandings_WithSingleRider_ShouldReturnFirstPosition()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var riderStats = new List<RiderStats>
        {
            new(riderId, seasonId, 100, 3, 5, 2, 1, 10, 8, 1, 1)
        };

        // Act
        var result = _service.CalculateRiderStandings(riderStats).ToList();

        // Assert
        result.Should().HaveCount(1);
        result[0].ChampionshipPosition.Should().Be(1);
        result[0].RiderId.Should().Be(riderId);
    }

    [Fact]
    public void CalculateRiderStandings_WithMultipleRiders_ShouldOrderByPointsDescending()
    {
        // Arrange
        var rider1Id = Guid.NewGuid();
        var rider2Id = Guid.NewGuid();
        var rider3Id = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var riderStats = new List<RiderStats>
        {
            new(rider2Id, seasonId, 80, 1, 3, 1, 1, 10, 8, 2, 0), // 2nd place
            new(rider1Id, seasonId, 150, 4, 7, 3, 2, 10, 8, 2, 0), // 1st place
            new(rider3Id, seasonId, 50, 0, 2, 0, 0, 10, 8, 2, 0)  // 3rd place
        };

        // Act
        var result = _service.CalculateRiderStandings(riderStats).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].RiderId.Should().Be(rider1Id);
        result[0].ChampionshipPosition.Should().Be(1);
        result[1].RiderId.Should().Be(rider2Id);
        result[1].ChampionshipPosition.Should().Be(2);
        result[2].RiderId.Should().Be(rider3Id);
        result[2].ChampionshipPosition.Should().Be(3);
    }

    [Fact]
    public void CalculateRiderStandings_WithTiedPointsButDifferentWins_ShouldOrderByWins()
    {
        // Arrange
        var rider1Id = Guid.NewGuid();
        var rider2Id = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var riderStats = new List<RiderStats>
        {
            new(rider1Id, seasonId, 100, 2, 5, 1, 1, 10, 8, 2, 0), // More wins
            new(rider2Id, seasonId, 100, 1, 4, 2, 2, 10, 8, 2, 0)  // Fewer wins
        };

        // Act
        var result = _service.CalculateRiderStandings(riderStats).ToList();

        // Assert
        result.Should().HaveCount(2);
        result[0].RiderId.Should().Be(rider1Id); // More wins, so first
        result[0].ChampionshipPosition.Should().Be(1);
        result[1].RiderId.Should().Be(rider2Id);
        result[1].ChampionshipPosition.Should().Be(2);
    }

    [Fact]
    public void CalculateRiderStandings_WithTiedPointsAndWinsButDifferentPodiums_ShouldOrderByPodiums()
    {
        // Arrange
        var rider1Id = Guid.NewGuid();
        var rider2Id = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var riderStats = new List<RiderStats>
        {
            new(rider1Id, seasonId, 100, 2, 6, 1, 1, 10, 8, 2, 0), // More podiums
            new(rider2Id, seasonId, 100, 2, 4, 2, 2, 10, 8, 2, 0)  // Fewer podiums
        };

        // Act
        var result = _service.CalculateRiderStandings(riderStats).ToList();

        // Assert
        result.Should().HaveCount(2);
        result[0].RiderId.Should().Be(rider1Id); // More podiums, so first
        result[0].ChampionshipPosition.Should().Be(1);
        result[1].RiderId.Should().Be(rider2Id);
        result[1].ChampionshipPosition.Should().Be(2);
    }

    [Fact]
    public void CalculateRiderStandings_WithCompletelyTiedRiders_ShouldHaveSamePosition()
    {
        // Arrange
        var rider1Id = Guid.NewGuid();
        var rider2Id = Guid.NewGuid();
        var rider3Id = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var riderStats = new List<RiderStats>
        {
            new(rider1Id, seasonId, 100, 2, 5, 1, 2, 10, 8, 2, 0), // Tied for 1st
            new(rider2Id, seasonId, 100, 2, 5, 1, 2, 10, 8, 2, 0), // Tied for 1st
            new(rider3Id, seasonId, 80, 1, 3, 0, 1, 10, 8, 2, 0)   // 3rd
        };

        // Act
        var result = _service.CalculateRiderStandings(riderStats).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].ChampionshipPosition.Should().Be(1); // Tied for 1st
        result[1].ChampionshipPosition.Should().Be(1); // Tied for 1st
        result[2].ChampionshipPosition.Should().Be(3); // Next available position (skipping 2)
    }

    #endregion

    #region CalculateTeamStandings Tests

    [Fact]
    public void CalculateTeamStandings_WithEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        var teamStats = new List<TeamStats>();

        // Act
        var result = _service.CalculateTeamStandings(teamStats);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void CalculateTeamStandings_WithSingleTeam_ShouldReturnFirstPosition()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var teamStats = new List<TeamStats>
        {
            new(teamId, seasonId, 200, 5, 8, 3, 2, 10)
        };

        // Act
        var result = _service.CalculateTeamStandings(teamStats).ToList();

        // Assert
        result.Should().HaveCount(1);
        result[0].ChampionshipPosition.Should().Be(1);
        result[0].TeamId.Should().Be(teamId);
    }

    [Fact]
    public void CalculateTeamStandings_WithMultipleTeams_ShouldOrderByPointsDescending()
    {
        // Arrange
        var team1Id = Guid.NewGuid();
        var team2Id = Guid.NewGuid();
        var team3Id = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var teamStats = new List<TeamStats>
        {
            new(team2Id, seasonId, 150, 3, 6, 2, 1, 10), // 2nd place
            new(team1Id, seasonId, 250, 6, 10, 4, 3, 10), // 1st place
            new(team3Id, seasonId, 100, 1, 4, 1, 1, 10)  // 3rd place
        };

        // Act
        var result = _service.CalculateTeamStandings(teamStats).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].TeamId.Should().Be(team1Id);
        result[0].ChampionshipPosition.Should().Be(1);
        result[1].TeamId.Should().Be(team2Id);
        result[1].ChampionshipPosition.Should().Be(2);
        result[2].TeamId.Should().Be(team3Id);
        result[2].ChampionshipPosition.Should().Be(3);
    }

    [Fact]
    public void CalculateTeamStandings_WithCompletelyTiedTeams_ShouldHaveSamePosition()
    {
        // Arrange
        var team1Id = Guid.NewGuid();
        var team2Id = Guid.NewGuid();
        var team3Id = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var teamStats = new List<TeamStats>
        {
            new(team1Id, seasonId, 200, 4, 8, 2, 3, 10), // Tied for 1st
            new(team2Id, seasonId, 200, 4, 8, 2, 3, 10), // Tied for 1st
            new(team3Id, seasonId, 150, 2, 5, 1, 2, 10)  // 3rd
        };

        // Act
        var result = _service.CalculateTeamStandings(teamStats).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].ChampionshipPosition.Should().Be(1); // Tied for 1st
        result[1].ChampionshipPosition.Should().Be(1); // Tied for 1st
        result[2].ChampionshipPosition.Should().Be(3); // Next available position (skipping 2)
    }

    #endregion

    #region UpdateRiderPosition Tests

    [Fact]
    public void UpdateRiderPosition_ShouldCreateNewRiderStatsWithUpdatedPosition()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var originalStats = new RiderStats(riderId, seasonId, 100, 3, 5, 2, 1, 10, 8, 1, 1, 0);
        var newPosition = 2;

        // Act
        var result = _service.UpdateRiderPosition(originalStats, newPosition);

        // Assert
        result.RiderId.Should().Be(riderId);
        result.SeasonId.Should().Be(seasonId);
        result.TotalPoints.Should().Be(100);
        result.Wins.Should().Be(3);
        result.Podiums.Should().Be(5);
        result.PolePositions.Should().Be(2);
        result.FastestLaps.Should().Be(1);
        result.RacesParticipated.Should().Be(10);
        result.RacesFinished.Should().Be(8);
        result.DidNotFinish.Should().Be(1);
        result.DidNotStart.Should().Be(1);
        result.ChampionshipPosition.Should().Be(newPosition);
    }

    #endregion

    #region UpdateTeamPosition Tests

    [Fact]
    public void UpdateTeamPosition_ShouldCreateNewTeamStatsWithUpdatedPosition()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var originalStats = new TeamStats(teamId, seasonId, 200, 5, 8, 3, 2, 10, 0);
        var newPosition = 3;

        // Act
        var result = _service.UpdateTeamPosition(originalStats, newPosition);

        // Assert
        result.TeamId.Should().Be(teamId);
        result.SeasonId.Should().Be(seasonId);
        result.TotalPoints.Should().Be(200);
        result.Wins.Should().Be(5);
        result.Podiums.Should().Be(8);
        result.PolePositions.Should().Be(3);
        result.FastestLaps.Should().Be(2);
        result.RacesParticipated.Should().Be(10);
        result.ChampionshipPosition.Should().Be(newPosition);
    }

    #endregion
}