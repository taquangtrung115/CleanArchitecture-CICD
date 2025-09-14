using DemoCICD.Domain.Abstractions.Entities;
using DemoCICD.Domain.Entities.MotoGP.ValueObjects;

namespace DemoCICD.Domain.Entities.MotoGP.RaceManagement;

public class RaceEntry : AuditableEntity<Guid>
{
    public Guid RaceId { get; private set; }
    public Guid RiderId { get; private set; }
    public Guid TeamId { get; private set; }
    public Guid BikeId { get; private set; }
    public int StartingGrid { get; private set; }
    public RaceResult? Result { get; private set; }
    public bool FastestLap { get; private set; }
    public TimeSpan? FastestLapTime { get; private set; }
    public string? Notes { get; private set; }

    public RaceEntry(Guid id, Guid raceId, Guid riderId, Guid teamId, Guid bikeId, int startingGrid, string? notes = null)
    {
        if (startingGrid <= 0)
            throw new ArgumentException("Starting grid position must be positive", nameof(startingGrid));

        Id = id;
        RaceId = raceId;
        RiderId = riderId;
        TeamId = teamId;
        BikeId = bikeId;
        StartingGrid = startingGrid;
        Notes = notes;
        FastestLap = false;
    }

    public static RaceEntry Create(Guid raceId, Guid riderId, Guid teamId, Guid bikeId, int startingGrid, string? notes = null)
    {
        return new RaceEntry(Guid.NewGuid(), raceId, riderId, teamId, bikeId, startingGrid, notes);
    }

    public void UpdateStartingGrid(int startingGrid)
    {
        if (startingGrid <= 0)
            throw new ArgumentException("Starting grid position must be positive", nameof(startingGrid));

        StartingGrid = startingGrid;
        SetUpdatedAudit();
    }

    public void SetRaceResult(RaceResult result)
    {
        Result = result ?? throw new ArgumentNullException(nameof(result));
        SetUpdatedAudit();
    }

    public void SetFastestLap(TimeSpan lapTime)
    {
        if (lapTime <= TimeSpan.Zero)
            throw new ArgumentException("Lap time must be positive", nameof(lapTime));

        FastestLap = true;
        FastestLapTime = lapTime;
        SetUpdatedAudit();
    }

    public void RemoveFastestLap()
    {
        FastestLap = false;
        FastestLapTime = null;
        SetUpdatedAudit();
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
        SetUpdatedAudit();
    }

    public bool IsFinisher => Result?.IsFinisher == true;
    public int Points => Result?.Points ?? 0;
}