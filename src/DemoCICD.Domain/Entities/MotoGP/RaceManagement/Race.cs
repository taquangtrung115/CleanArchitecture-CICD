using DemoCICD.Domain.Abstractions.Entities;
using DemoCICD.Domain.Entities.MotoGP.ValueObjects;

namespace DemoCICD.Domain.Entities.MotoGP.RaceManagement;

public enum RaceStatus
{
    Upcoming,
    InProgress,
    Completed,
    Cancelled,
    Postponed
}

public class Race : AuditableEntity<Guid>
{
    public Guid SeasonId { get; private set; }
    public string Name { get; private set; }
    public string CircuitName { get; private set; }
    public Country Country { get; private set; }
    public DateTime RaceDate { get; private set; }
    public RaceSchedule Schedule { get; private set; }
    public RaceStatus Status { get; private set; }
    public int RoundNumber { get; private set; }
    public string? Description { get; private set; }
    public string? WeatherConditions { get; private set; }
    public decimal CircuitLength { get; private set; }
    public int NumberOfLaps { get; private set; }

    private readonly List<RaceEntry> _raceEntries = new();
    public virtual IReadOnlyList<RaceEntry> RaceEntries => _raceEntries.AsReadOnly();

    // EF Core requires a parameterless constructor
    protected Race() { }

    public Race(Guid id, Guid seasonId, string name, string circuitName, Country country, 
               DateTime raceDate, RaceSchedule schedule, int roundNumber, decimal circuitLength, 
               int numberOfLaps, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Race name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(circuitName))
            throw new ArgumentException("Circuit name cannot be empty", nameof(circuitName));
        if (roundNumber <= 0)
            throw new ArgumentException("Round number must be positive", nameof(roundNumber));
        if (circuitLength <= 0)
            throw new ArgumentException("Circuit length must be positive", nameof(circuitLength));
        if (numberOfLaps <= 0)
            throw new ArgumentException("Number of laps must be positive", nameof(numberOfLaps));

        Id = id;
        SeasonId = seasonId;
        Name = name;
        CircuitName = circuitName;
        Country = country ?? throw new ArgumentNullException(nameof(country));
        RaceDate = raceDate;
        Schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
        RoundNumber = roundNumber;
        CircuitLength = circuitLength;
        NumberOfLaps = numberOfLaps;
        Description = description;
        Status = RaceStatus.Upcoming;
    }

    public static Race Create(Guid seasonId, string name, string circuitName, Country country, 
                             DateTime raceDate, RaceSchedule schedule, int roundNumber, 
                             decimal circuitLength, int numberOfLaps, string? description = null)
    {
        return new Race(Guid.NewGuid(), seasonId, name, circuitName, country, raceDate, 
                       schedule, roundNumber, circuitLength, numberOfLaps, description);
    }

    public void UpdateDetails(string name, string circuitName, Country country, DateTime raceDate, 
                             RaceSchedule schedule, decimal circuitLength, int numberOfLaps, string? description = null)
    {
        if (Status == RaceStatus.Completed)
            throw new InvalidOperationException("Cannot update completed race");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Race name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(circuitName))
            throw new ArgumentException("Circuit name cannot be empty", nameof(circuitName));

        Name = name;
        CircuitName = circuitName;
        Country = country ?? throw new ArgumentNullException(nameof(country));
        RaceDate = raceDate;
        Schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
        CircuitLength = circuitLength;
        NumberOfLaps = numberOfLaps;
        Description = description;
        SetUpdatedAudit();
    }

    public void StartRace()
    {
        if (Status != RaceStatus.Upcoming)
            throw new InvalidOperationException($"Cannot start race in {Status} status");

        Status = RaceStatus.InProgress;
        SetUpdatedAudit();
    }

    public void CompleteRace(string? weatherConditions = null)
    {
        if (Status != RaceStatus.InProgress)
            throw new InvalidOperationException($"Cannot complete race in {Status} status");

        Status = RaceStatus.Completed;
        WeatherConditions = weatherConditions;
        SetUpdatedAudit();
    }

    public void PostponeRace(DateTime newRaceDate, RaceSchedule newSchedule)
    {
        if (Status == RaceStatus.Completed)
            throw new InvalidOperationException("Cannot postpone completed race");

        Status = RaceStatus.Postponed;
        RaceDate = newRaceDate;
        Schedule = newSchedule;
        SetUpdatedAudit();
    }

    public void CancelRace(string reason)
    {
        if (Status == RaceStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed race");

        Status = RaceStatus.Cancelled;
        Description = $"{Description}\nCancelled: {reason}";
        SetUpdatedAudit();
    }

    public void AddRaceEntry(RaceEntry raceEntry)
    {
        if (Status == RaceStatus.Completed)
            throw new InvalidOperationException("Cannot add entries to completed race");

        if (_raceEntries.Any(e => e.RiderId == raceEntry.RiderId))
            throw new InvalidOperationException("Rider already has an entry in this race");

        _raceEntries.Add(raceEntry);
        SetUpdatedAudit();
    }

    public void RemoveRaceEntry(Guid riderEntryId)
    {
        if (Status == RaceStatus.Completed)
            throw new InvalidOperationException("Cannot remove entries from completed race");

        var entry = _raceEntries.FirstOrDefault(e => e.Id == riderEntryId);
        if (entry != null)
        {
            _raceEntries.Remove(entry);
            SetUpdatedAudit();
        }
    }

    public RaceEntry? GetRaceEntry(Guid riderId) => _raceEntries.FirstOrDefault(e => e.RiderId == riderId);
    public int GetTotalEntries() => _raceEntries.Count;
    public IEnumerable<RaceEntry> GetFinishedEntries() => _raceEntries.Where(e => e.Result?.IsFinisher == true);
}
