using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.MotoGP.RaceManagement;

public enum SeasonStatus
{
    Upcoming,
    InProgress,
    Completed,
    Cancelled
}

public class Season : AuditableEntity<Guid>
{
    public int Year { get; private set; }
    public string Name { get; private set; }
    public SeasonStatus Status { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string? Description { get; private set; }
    public bool IsCurrentSeason { get; private set; }

    private readonly List<Race> _races = new();
    public virtual IReadOnlyList<Race> Races => _races.AsReadOnly();

    public Season(Guid id, int year, string name, DateTime startDate, DateTime endDate, string? description = null)
    {
        if (year < 1949) // MotoGP started in 1949
            throw new ArgumentException("Season year cannot be before 1949", nameof(year));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Season name cannot be empty", nameof(name));
        if (startDate >= endDate)
            throw new ArgumentException("Start date must be before end date");

        Id = id;
        Year = year;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        Status = SeasonStatus.Upcoming;
        IsCurrentSeason = false;
    }

    public static Season Create(int year, string name, DateTime startDate, DateTime endDate, string? description = null)
    {
        return new Season(Guid.NewGuid(), year, name, startDate, endDate, description);
    }

    public void UpdateDetails(string name, DateTime startDate, DateTime endDate, string? description = null)
    {
        if (Status == SeasonStatus.Completed)
            throw new InvalidOperationException("Cannot update completed season");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Season name cannot be empty", nameof(name));
        if (startDate >= endDate)
            throw new ArgumentException("Start date must be before end date");

        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        SetUpdatedAudit();
    }

    public void StartSeason()
    {
        if (Status != SeasonStatus.Upcoming)
            throw new InvalidOperationException($"Cannot start season in {Status} status");

        Status = SeasonStatus.InProgress;
        SetUpdatedAudit();
    }

    public void CompleteSeason()
    {
        if (Status != SeasonStatus.InProgress)
            throw new InvalidOperationException($"Cannot complete season in {Status} status");

        Status = SeasonStatus.Completed;
        SetUpdatedAudit();
    }

    public void CancelSeason(string reason)
    {
        if (Status == SeasonStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed season");

        Status = SeasonStatus.Cancelled;
        Description = $"{Description}\nCancelled: {reason}";
        SetUpdatedAudit();
    }

    public void SetAsCurrentSeason()
    {
        IsCurrentSeason = true;
        SetUpdatedAudit();
    }

    public void UnsetAsCurrentSeason()
    {
        IsCurrentSeason = false;
        SetUpdatedAudit();
    }

    public void AddRace(Race race)
    {
        if (Status == SeasonStatus.Completed)
            throw new InvalidOperationException("Cannot add races to completed season");

        if (race.RaceDate < StartDate || race.RaceDate > EndDate)
            throw new ArgumentException("Race date must be within season dates");

        if (_races.Any(r => r.CircuitName == race.CircuitName && r.RaceDate.Date == race.RaceDate.Date))
            throw new InvalidOperationException($"Race at {race.CircuitName} already exists for this date");

        _races.Add(race);
        SetUpdatedAudit();
    }

    public void RemoveRace(Guid raceId)
    {
        if (Status == SeasonStatus.Completed)
            throw new InvalidOperationException("Cannot remove races from completed season");

        var race = _races.FirstOrDefault(r => r.Id == raceId);
        if (race != null)
        {
            _races.Remove(race);
            SetUpdatedAudit();
        }
    }

    public int GetTotalRaces() => _races.Count;
    public int GetCompletedRaces() => _races.Count(r => r.Status == RaceStatus.Completed);
    public Race? GetNextRace() => _races.Where(r => r.Status == RaceStatus.Upcoming).OrderBy(r => r.RaceDate).FirstOrDefault();
}
