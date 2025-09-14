using DemoCICD.Domain.Entities.MotoGP.ValueObjects;

namespace DemoCICD.Domain.Services.MotoGP;

public interface IStandingsCalculationService
{
    IEnumerable<RiderStats> CalculateRiderStandings(IEnumerable<RiderStats> riderStats);
    IEnumerable<TeamStats> CalculateTeamStandings(IEnumerable<TeamStats> teamStats);
    RiderStats UpdateRiderPosition(RiderStats riderStats, int position);
    TeamStats UpdateTeamPosition(TeamStats teamStats, int position);
}

public class StandingsCalculationService : IStandingsCalculationService
{
    public IEnumerable<RiderStats> CalculateRiderStandings(IEnumerable<RiderStats> riderStats)
    {
        var sortedStats = riderStats
            .OrderByDescending(r => r.TotalPoints)
            .ThenByDescending(r => r.Wins)
            .ThenByDescending(r => r.Podiums)
            .ThenByDescending(r => r.FastestLaps)
            .ToList();

        var standings = new List<RiderStats>();
        var currentPosition = 1;

        for (int i = 0; i < sortedStats.Count; i++)
        {
            var rider = sortedStats[i];
            
            // Check if tied with previous rider
            if (i > 0 && IsRiderTied(sortedStats[i - 1], rider))
            {
                // Keep same position as previous rider
                standings.Add(UpdateRiderPosition(rider, standings[i - 1].ChampionshipPosition));
            }
            else
            {
                standings.Add(UpdateRiderPosition(rider, currentPosition));
            }
            
            currentPosition = i + 2; // Next available position
        }

        return standings;
    }

    public IEnumerable<TeamStats> CalculateTeamStandings(IEnumerable<TeamStats> teamStats)
    {
        var sortedStats = teamStats
            .OrderByDescending(t => t.TotalPoints)
            .ThenByDescending(t => t.Wins)
            .ThenByDescending(t => t.Podiums)
            .ThenByDescending(t => t.FastestLaps)
            .ToList();

        var standings = new List<TeamStats>();
        var currentPosition = 1;

        for (int i = 0; i < sortedStats.Count; i++)
        {
            var team = sortedStats[i];
            
            // Check if tied with previous team
            if (i > 0 && IsTeamTied(sortedStats[i - 1], team))
            {
                // Keep same position as previous team
                standings.Add(UpdateTeamPosition(team, standings[i - 1].ChampionshipPosition));
            }
            else
            {
                standings.Add(UpdateTeamPosition(team, currentPosition));
            }
            
            currentPosition = i + 2; // Next available position
        }

        return standings;
    }

    public RiderStats UpdateRiderPosition(RiderStats riderStats, int position)
    {
        return new RiderStats(
            riderStats.RiderId,
            riderStats.SeasonId,
            riderStats.TotalPoints,
            riderStats.Wins,
            riderStats.Podiums,
            riderStats.PolePositions,
            riderStats.FastestLaps,
            riderStats.RacesParticipated,
            riderStats.RacesFinished,
            riderStats.DidNotFinish,
            riderStats.DidNotStart,
            position
        );
    }

    public TeamStats UpdateTeamPosition(TeamStats teamStats, int position)
    {
        return new TeamStats(
            teamStats.TeamId,
            teamStats.SeasonId,
            teamStats.TotalPoints,
            teamStats.Wins,
            teamStats.Podiums,
            teamStats.PolePositions,
            teamStats.FastestLaps,
            teamStats.RacesParticipated,
            position
        );
    }

    private bool IsRiderTied(RiderStats rider1, RiderStats rider2)
    {
        return rider1.TotalPoints == rider2.TotalPoints &&
               rider1.Wins == rider2.Wins &&
               rider1.Podiums == rider2.Podiums &&
               rider1.FastestLaps == rider2.FastestLaps;
    }

    private bool IsTeamTied(TeamStats team1, TeamStats team2)
    {
        return team1.TotalPoints == team2.TotalPoints &&
               team1.Wins == team2.Wins &&
               team1.Podiums == team2.Podiums &&
               team1.FastestLaps == team2.FastestLaps;
    }
}