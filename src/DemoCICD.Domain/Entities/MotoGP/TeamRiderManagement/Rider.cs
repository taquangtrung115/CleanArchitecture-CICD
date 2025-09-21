using DemoCICD.Domain.Abstractions.Entities;
using DemoCICD.Domain.Entities.MotoGP.ValueObjects;

namespace DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;

public class Rider : AuditableEntity<Guid>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName => $"{FirstName} {LastName}";
    public int RacingNumber { get; private set; }
    public Country Nationality { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Guid? CurrentTeamId { get; private set; }
    public string? Nickname { get; private set; }
    public string? Photo { get; private set; }
    public decimal Height { get; private set; } // in cm
    public decimal Weight { get; private set; } // in kg
    public bool IsActive { get; private set; }
    public DateTime? DebutDate { get; private set; }
    public DateTime? RetirementDate { get; private set; }

    private readonly List<RiderTeamHistory> _teamHistory = new();
    public virtual IReadOnlyList<RiderTeamHistory> TeamHistory => _teamHistory.AsReadOnly();

    // EF Core requires a parameterless constructor
    protected Rider() { }

    public Rider(Guid id, string firstName, string lastName, int racingNumber, Country nationality, 
                DateTime dateOfBirth, decimal height, decimal weight, string? nickname = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));
        if (racingNumber <= 0 || racingNumber > 99)
            throw new ArgumentException("Racing number must be between 1 and 99", nameof(racingNumber));
        if (dateOfBirth > DateTime.Today.AddYears(-16)) // Minimum age for MotoGP
            throw new ArgumentException("Rider must be at least 16 years old");
        if (height <= 0)
            throw new ArgumentException("Height must be positive", nameof(height));
        if (weight <= 0)
            throw new ArgumentException("Weight must be positive", nameof(weight));

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        RacingNumber = racingNumber;
        Nationality = nationality ?? throw new ArgumentNullException(nameof(nationality));
        DateOfBirth = dateOfBirth;
        Height = height;
        Weight = weight;
        Nickname = nickname;
        IsActive = true;
    }

    public static Rider Create(string firstName, string lastName, int racingNumber, Country nationality, 
                              DateTime dateOfBirth, decimal height, decimal weight, string? nickname = null)
    {
        return new Rider(Guid.NewGuid(), firstName, lastName, racingNumber, nationality, 
                        dateOfBirth, height, weight, nickname);
    }

    public void UpdatePersonalInfo(string firstName, string lastName, string? nickname, decimal height, decimal weight)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));
        if (height <= 0)
            throw new ArgumentException("Height must be positive", nameof(height));
        if (weight <= 0)
            throw new ArgumentException("Weight must be positive", nameof(weight));

        FirstName = firstName;
        LastName = lastName;
        Nickname = nickname;
        Height = height;
        Weight = weight;
        SetUpdatedAudit();
    }

    public void UpdateRacingNumber(int racingNumber)
    {
        if (racingNumber <= 0 || racingNumber > 99)
            throw new ArgumentException("Racing number must be between 1 and 99", nameof(racingNumber));

        RacingNumber = racingNumber;
        SetUpdatedAudit();
    }

    public void UpdatePhoto(string? photo)
    {
        Photo = photo;
        SetUpdatedAudit();
    }

    public void JoinTeam(Guid teamId, DateTime joinDate, Guid seasonId)
    {
        if (!IsActive)
            throw new InvalidOperationException("Inactive rider cannot join a team");

        // End current team relationship if exists
        var currentTeamHistory = _teamHistory.FirstOrDefault(h => h.EndDate == null);
        currentTeamHistory?.EndTeamRelationship(joinDate.AddDays(-1));

        CurrentTeamId = teamId;
        _teamHistory.Add(new RiderTeamHistory(Guid.NewGuid(), Id, teamId, seasonId, joinDate));
        
        if (DebutDate == null)
            DebutDate = joinDate;

        SetUpdatedAudit();
    }

    public void LeaveTeam(DateTime leaveDate)
    {
        if (CurrentTeamId == null)
            throw new InvalidOperationException("Rider is not currently in any team");

        var currentTeamHistory = _teamHistory.FirstOrDefault(h => h.EndDate == null);
        currentTeamHistory?.EndTeamRelationship(leaveDate);

        CurrentTeamId = null;
        SetUpdatedAudit();
    }

    public void Retire(DateTime retirementDate)
    {
        if (!IsActive)
            throw new InvalidOperationException("Rider is already retired");

        IsActive = false;
        RetirementDate = retirementDate;

        // End current team relationship if exists
        if (CurrentTeamId != null)
            LeaveTeam(retirementDate);

        SetUpdatedAudit();
    }

    public void Comeback()
    {
        if (IsActive)
            throw new InvalidOperationException("Rider is already active");

        IsActive = true;
        RetirementDate = null;
        SetUpdatedAudit();
    }

    public int GetAge() => DateTime.Today.Year - DateOfBirth.Year - (DateTime.Today.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
    public int GetYearsInMotoGP() => DebutDate.HasValue ? DateTime.Today.Year - DebutDate.Value.Year : 0;
    public bool IsCurrentlyInTeam => CurrentTeamId.HasValue && IsActive;
    public RiderTeamHistory? GetCurrentTeamHistory() => _teamHistory.FirstOrDefault(h => h.EndDate == null);
}
