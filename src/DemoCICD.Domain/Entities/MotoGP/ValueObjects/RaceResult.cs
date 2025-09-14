namespace DemoCICD.Domain.Entities.MotoGP.ValueObjects;

public class RaceResult
{
    public int Position { get; private set; }
    public int Points { get; private set; }
    public TimeSpan? RaceTime { get; private set; }
    public TimeSpan? GapToWinner { get; private set; }
    public bool DidNotFinish { get; private set; }
    public bool DidNotStart { get; private set; }
    public bool Disqualified { get; private set; }
    public string? Reason { get; private set; }

    public RaceResult(int position, int points, TimeSpan? raceTime = null, TimeSpan? gapToWinner = null, 
                     bool didNotFinish = false, bool didNotStart = false, bool disqualified = false, string? reason = null)
    {
        if (position <= 0 && !didNotFinish && !didNotStart && !disqualified)
            throw new ArgumentException("Position must be greater than 0 for finished races", nameof(position));
        if (points < 0)
            throw new ArgumentException("Points cannot be negative", nameof(points));

        Position = position;
        Points = points;
        RaceTime = raceTime;
        GapToWinner = gapToWinner;
        DidNotFinish = didNotFinish;
        DidNotStart = didNotStart;
        Disqualified = disqualified;
        Reason = reason;
    }

    public bool IsFinisher => !DidNotFinish && !DidNotStart && !Disqualified;

    public override bool Equals(object? obj)
    {
        return obj is RaceResult result && 
               Position == result.Position && 
               Points == result.Points &&
               RaceTime == result.RaceTime;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position, Points, RaceTime);
    }

    public override string ToString()
    {
        if (DidNotStart) return "DNS";
        if (DidNotFinish) return "DNF";
        if (Disqualified) return "DSQ";
        return $"P{Position} ({Points} pts)";
    }
}