namespace DemoCICD.Domain.Entities.MotoGP.ValueObjects;

public class RiderStats
{
    public Guid RiderId { get; private set; }
    public Guid SeasonId { get; private set; }
    public int TotalPoints { get; private set; }
    public int Wins { get; private set; }
    public int Podiums { get; private set; }
    public int PolePositions { get; private set; }
    public int FastestLaps { get; private set; }
    public int RacesParticipated { get; private set; }
    public int RacesFinished { get; private set; }
    public int DidNotFinish { get; private set; }
    public int DidNotStart { get; private set; }
    public int ChampionshipPosition { get; private set; }

    public RiderStats(Guid riderId, Guid seasonId, int totalPoints = 0, int wins = 0, int podiums = 0, 
                     int polePositions = 0, int fastestLaps = 0, int racesParticipated = 0, 
                     int racesFinished = 0, int didNotFinish = 0, int didNotStart = 0, int championshipPosition = 0)
    {
        if (totalPoints < 0) throw new ArgumentException("Total points cannot be negative");
        if (wins < 0) throw new ArgumentException("Wins cannot be negative");
        if (podiums < 0) throw new ArgumentException("Podiums cannot be negative");
        if (polePositions < 0) throw new ArgumentException("Pole positions cannot be negative");
        if (fastestLaps < 0) throw new ArgumentException("Fastest laps cannot be negative");
        if (racesParticipated < 0) throw new ArgumentException("Races participated cannot be negative");
        if (racesFinished < 0) throw new ArgumentException("Races finished cannot be negative");
        if (didNotFinish < 0) throw new ArgumentException("DNF count cannot be negative");
        if (didNotStart < 0) throw new ArgumentException("DNS count cannot be negative");
        if (wins > podiums) throw new ArgumentException("Wins cannot exceed podiums");
        if (podiums > racesFinished) throw new ArgumentException("Podiums cannot exceed races finished");

        RiderId = riderId;
        SeasonId = seasonId;
        TotalPoints = totalPoints;
        Wins = wins;
        Podiums = podiums;
        PolePositions = polePositions;
        FastestLaps = fastestLaps;
        RacesParticipated = racesParticipated;
        RacesFinished = racesFinished;
        DidNotFinish = didNotFinish;
        DidNotStart = didNotStart;
        ChampionshipPosition = championshipPosition;
    }

    public decimal FinishingRate => RacesParticipated > 0 ? (decimal)RacesFinished / RacesParticipated * 100 : 0;
    public decimal PodiumRate => RacesFinished > 0 ? (decimal)Podiums / RacesFinished * 100 : 0;
    public decimal WinRate => RacesFinished > 0 ? (decimal)Wins / RacesFinished * 100 : 0;
    public decimal PointsPerRace => RacesParticipated > 0 ? (decimal)TotalPoints / RacesParticipated : 0;

    public override bool Equals(object? obj)
    {
        return obj is RiderStats stats && 
               RiderId == stats.RiderId && 
               SeasonId == stats.SeasonId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RiderId, SeasonId);
    }

    public override string ToString()
    {
        return $"Points: {TotalPoints}, Wins: {Wins}, Podiums: {Podiums}, Position: {ChampionshipPosition}";
    }
}