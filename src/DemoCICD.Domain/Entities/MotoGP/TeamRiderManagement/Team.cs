using DemoCICD.Domain.Abstractions.Entities;
using DemoCICD.Domain.Entities.MotoGP.ValueObjects;

namespace DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;

public class Team : AuditableEntity<Guid>
{
    public string Name { get; private set; }
    public string ShortName { get; private set; }
    public Country Country { get; private set; }
    public string? Logo { get; private set; }
    public string? Website { get; private set; }
    public DateTime FoundedYear { get; private set; }
    public string? Description { get; private set; }
    public string? PrimaryColor { get; private set; }
    public string? SecondaryColor { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<Rider> _riders = new();
    public virtual IReadOnlyList<Rider> Riders => _riders.AsReadOnly();

    private readonly List<Bike> _bikes = new();
    public virtual IReadOnlyList<Bike> Bikes => _bikes.AsReadOnly();

    // EF Core requires a parameterless constructor
    protected Team() { }

    public Team(Guid id, string name, string shortName, Country country, DateTime foundedYear, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Team name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(shortName))
            throw new ArgumentException("Team short name cannot be empty", nameof(shortName));

        Id = id;
        Name = name;
        ShortName = shortName;
        Country = country ?? throw new ArgumentNullException(nameof(country));
        FoundedYear = foundedYear;
        Description = description;
        IsActive = true;
    }

    public static Team Create(string name, string shortName, Country country, DateTime foundedYear, string? description = null)
    {
        return new Team(Guid.NewGuid(), name, shortName, country, foundedYear, description);
    }

    public void UpdateDetails(string name, string shortName, Country country, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Team name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(shortName))
            throw new ArgumentException("Team short name cannot be empty", nameof(shortName));

        Name = name;
        ShortName = shortName;
        Country = country ?? throw new ArgumentNullException(nameof(country));
        Description = description;
        SetUpdatedAudit();
    }

    public void UpdateVisualIdentity(string? logo, string? website, string? primaryColor, string? secondaryColor)
    {
        Logo = logo;
        Website = website;
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
        SetUpdatedAudit();
    }

    public void ActivateTeam()
    {
        IsActive = true;
        SetUpdatedAudit();
    }

    public void DeactivateTeam()
    {
        IsActive = false;
        SetUpdatedAudit();
    }

    public void AddRider(Rider rider)
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot add riders to inactive team");

        if (_riders.Any(r => r.Id == rider.Id))
            throw new InvalidOperationException("Rider is already in this team");

        if (_riders.Count >= 2) // MotoGP teams typically have 2 riders
            throw new InvalidOperationException("Team already has maximum number of riders");

        _riders.Add(rider);
        SetUpdatedAudit();
    }

    public void RemoveRider(Guid riderId)
    {
        var rider = _riders.FirstOrDefault(r => r.Id == riderId);
        if (rider != null)
        {
            _riders.Remove(rider);
            SetUpdatedAudit();
        }
    }

    public void AddBike(Bike bike)
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot add bikes to inactive team");

        if (_bikes.Any(b => b.Id == bike.Id))
            throw new InvalidOperationException("Bike is already assigned to this team");

        _bikes.Add(bike);
        SetUpdatedAudit();
    }

    public void RemoveBike(Guid bikeId)
    {
        var bike = _bikes.FirstOrDefault(b => b.Id == bikeId);
        if (bike != null)
        {
            _bikes.Remove(bike);
            SetUpdatedAudit();
        }
    }

    public Rider? GetRider(Guid riderId) => _riders.FirstOrDefault(r => r.Id == riderId);
    public Bike? GetBike(Guid bikeId) => _bikes.FirstOrDefault(b => b.Id == bikeId);
    public int GetActiveRidersCount() => _riders.Count(r => r.IsActive);
    public int GetBikesCount() => _bikes.Count;
}
