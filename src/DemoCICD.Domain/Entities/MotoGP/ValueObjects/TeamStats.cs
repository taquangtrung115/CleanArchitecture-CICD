namespace DemoCICD.Domain.Entities.MotoGP.ValueObjects;

public class TeamStats
{
    public Guid TeamId { get; private set; }
    public Guid SeasonId { get; private set; }
    public int TotalPoints { get; private set; }
    public int Wins { get; private set; }
    public int Podiums { get; private set; }
    public int PolePositions { get; private set; }
    public int FastestLaps { get; private set; }
    public int RacesParticipated { get; private set; }
    public int ChampionshipPosition { get; private set; }

    public TeamStats(Guid teamId, Guid seasonId, int totalPoints = 0, int wins = 0, int podiums = 0, 
                    int polePositions = 0, int fastestLaps = 0, int racesParticipated = 0, int championshipPosition = 0)
    {
        if (totalPoints < 0) throw new ArgumentException("Total points cannot be negative");
        if (wins < 0) throw new ArgumentException("Wins cannot be negative");
        if (podiums < 0) throw new ArgumentException("Podiums cannot be negative");
        if (polePositions < 0) throw new ArgumentException("Pole positions cannot be negative");
        if (fastestLaps < 0) throw new ArgumentException("Fastest laps cannot be negative");
        if (racesParticipated < 0) throw new ArgumentException("Races participated cannot be negative");

        TeamId = teamId;
        SeasonId = seasonId;
        TotalPoints = totalPoints;
        Wins = wins;
        Podiums = podiums;
        PolePositions = polePositions;
        FastestLaps = fastestLaps;
        RacesParticipated = racesParticipated;
        ChampionshipPosition = championshipPosition;
    }

    public decimal PointsPerRace => RacesParticipated > 0 ? (decimal)TotalPoints / RacesParticipated : 0;
    public decimal WinRate => RacesParticipated > 0 ? (decimal)Wins / RacesParticipated * 100 : 0;
    public decimal PodiumRate => RacesParticipated > 0 ? (decimal)Podiums / RacesParticipated * 100 : 0;

    public override bool Equals(object? obj)
    {
        return obj is TeamStats stats && 
               TeamId == stats.TeamId && 
               SeasonId == stats.SeasonId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TeamId, SeasonId);
    }

    public override string ToString()
    {
        return $"Points: {TotalPoints}, Wins: {Wins}, Podiums: {Podiums}, Position: {ChampionshipPosition}";
    }
}