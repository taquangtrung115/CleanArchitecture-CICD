using DemoCICD.Domain.Entities.MotoGP.ValueObjects;

namespace DemoCICD.Domain.Services.MotoGP;

public interface IPointsCalculationService
{
    int CalculateRacePoints(int position, bool didNotFinish = false, bool didNotStart = false, bool disqualified = false);
    int CalculateSprintPoints(int position, bool didNotFinish = false, bool didNotStart = false, bool disqualified = false);
    RiderStats CalculateRiderStats(Guid riderId, Guid seasonId, IEnumerable<RaceResult> raceResults);
    TeamStats CalculateTeamStats(Guid teamId, Guid seasonId, IEnumerable<RiderStats> riderStats);
}

public class PointsCalculationService : IPointsCalculationService
{
    // Standard MotoGP points system
    private readonly Dictionary<int, int> _racePointsSystem = new()
    {
        { 1, 25 }, { 2, 20 }, { 3, 16 }, { 4, 13 }, { 5, 11 }, { 6, 10 }, { 7, 9 }, { 8, 8 },
        { 9, 7 }, { 10, 6 }, { 11, 5 }, { 12, 4 }, { 13, 3 }, { 14, 2 }, { 15, 1 }
    };

    // Sprint points system (usually half of race points)
    private readonly Dictionary<int, int> _sprintPointsSystem = new()
    {
        { 1, 12 }, { 2, 9 }, { 3, 7 }, { 4, 6 }, { 5, 5 }, { 6, 4 }, { 7, 3 }, { 8, 2 }, { 9, 1 }
    };

    public int CalculateRacePoints(int position, bool didNotFinish = false, bool didNotStart = false, bool disqualified = false)
    {
        if (didNotFinish || didNotStart || disqualified)
            return 0;

        return _racePointsSystem.GetValueOrDefault(position, 0);
    }

    public int CalculateSprintPoints(int position, bool didNotFinish = false, bool didNotStart = false, bool disqualified = false)
    {
        if (didNotFinish || didNotStart || disqualified)
            return 0;

        return _sprintPointsSystem.GetValueOrDefault(position, 0);
    }

    public RiderStats CalculateRiderStats(Guid riderId, Guid seasonId, IEnumerable<RaceResult> raceResults)
    {
        var results = raceResults.ToList();
        
        var totalPoints = results.Sum(r => r.Points);
        var wins = results.Count(r => r.Position == 1 && r.IsFinisher);
        var podiums = results.Count(r => r.Position <= 3 && r.IsFinisher);
        var racesParticipated = results.Count;
        var racesFinished = results.Count(r => r.IsFinisher);
        var didNotFinish = results.Count(r => r.DidNotFinish);
        var didNotStart = results.Count(r => r.DidNotStart);

        return new RiderStats(riderId, seasonId, totalPoints, wins, podiums, 0, 0, 
                             racesParticipated, racesFinished, didNotFinish, didNotStart);
    }

    public TeamStats CalculateTeamStats(Guid teamId, Guid seasonId, IEnumerable<RiderStats> riderStats)
    {
        var stats = riderStats.ToList();
        
        var totalPoints = stats.Sum(s => s.TotalPoints);
        var wins = stats.Sum(s => s.Wins);
        var podiums = stats.Sum(s => s.Podiums);
        var polePositions = stats.Sum(s => s.PolePositions);
        var fastestLaps = stats.Sum(s => s.FastestLaps);
        var racesParticipated = stats.Max(s => s.RacesParticipated); // Team participates if any rider participates

        return new TeamStats(teamId, seasonId, totalPoints, wins, podiums, 
                           polePositions, fastestLaps, racesParticipated);
    }
}