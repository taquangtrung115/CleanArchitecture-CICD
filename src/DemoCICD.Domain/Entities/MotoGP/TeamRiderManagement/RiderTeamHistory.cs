using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;

public class RiderTeamHistory : AuditableEntity<Guid>
{
    public Guid RiderId { get; private set; }
    public Guid TeamId { get; private set; }
    public Guid SeasonId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Notes { get; private set; }

    public RiderTeamHistory(Guid id, Guid riderId, Guid teamId, Guid seasonId, DateTime startDate, string? notes = null)
    {
        Id = id;
        RiderId = riderId;
        TeamId = teamId;
        SeasonId = seasonId;
        StartDate = startDate;
        Notes = notes;
    }

    public static RiderTeamHistory Create(Guid riderId, Guid teamId, Guid seasonId, DateTime startDate, string? notes = null)
    {
        return new RiderTeamHistory(Guid.NewGuid(), riderId, teamId, seasonId, startDate, notes);
    }

    public void EndTeamRelationship(DateTime endDate)
    {
        if (EndDate.HasValue)
            throw new InvalidOperationException("Team relationship has already ended");
        if (endDate < StartDate)
            throw new ArgumentException("End date cannot be before start date", nameof(endDate));

        EndDate = endDate;
        SetUpdatedAudit();
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
        SetUpdatedAudit();
    }

    public bool IsCurrentRelationship => !EndDate.HasValue;
    public TimeSpan Duration => (EndDate ?? DateTime.Today) - StartDate;
}