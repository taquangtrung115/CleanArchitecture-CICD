using DemoCICD.Domain.Abstractions.Entities;
using DemoCICD.Domain.Entities.MotoGP.ValueObjects;

namespace DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;

public class Bike : AuditableEntity<Guid>
{
    public string Manufacturer { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public Guid? TeamId { get; private set; }
    public BikeSpec Specifications { get; private set; }
    public string? ChassisNumber { get; private set; }
    public string? EngineNumber { get; private set; }
    public string? Livery { get; private set; } // Color scheme/design
    public bool IsActive { get; private set; }
    public string? Notes { get; private set; }

    // EF Core requires a parameterless constructor
    protected Bike() { }

    public Bike(Guid id, string manufacturer, string model, int year, BikeSpec specifications, 
               string? chassisNumber = null, string? engineNumber = null)
    {
        if (string.IsNullOrWhiteSpace(manufacturer))
            throw new ArgumentException("Manufacturer cannot be empty", nameof(manufacturer));
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model cannot be empty", nameof(model));
        if (year < 1949 || year > DateTime.Now.Year + 1) // MotoGP started in 1949
            throw new ArgumentException("Invalid bike year", nameof(year));

        Id = id;
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        Specifications = specifications ?? throw new ArgumentNullException(nameof(specifications));
        ChassisNumber = chassisNumber;
        EngineNumber = engineNumber;
        IsActive = true;
    }

    public static Bike Create(string manufacturer, string model, int year, BikeSpec specifications, 
                             string? chassisNumber = null, string? engineNumber = null)
    {
        return new Bike(Guid.NewGuid(), manufacturer, model, year, specifications, chassisNumber, engineNumber);
    }

    public void UpdateDetails(string manufacturer, string model, int year, BikeSpec specifications)
    {
        if (string.IsNullOrWhiteSpace(manufacturer))
            throw new ArgumentException("Manufacturer cannot be empty", nameof(manufacturer));
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model cannot be empty", nameof(model));
        if (year < 1949 || year > DateTime.Now.Year + 1)
            throw new ArgumentException("Invalid bike year", nameof(year));

        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        Specifications = specifications ?? throw new ArgumentNullException(nameof(specifications));
        SetUpdatedAudit();
    }

    public void UpdateIdentification(string? chassisNumber, string? engineNumber)
    {
        ChassisNumber = chassisNumber;
        EngineNumber = engineNumber;
        SetUpdatedAudit();
    }

    public void UpdateLivery(string? livery)
    {
        Livery = livery;
        SetUpdatedAudit();
    }

    public void AssignToTeam(Guid teamId)
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot assign inactive bike to team");

        TeamId = teamId;
        SetUpdatedAudit();
    }

    public void UnassignFromTeam()
    {
        TeamId = null;
        SetUpdatedAudit();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdatedAudit();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAudit();
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
        SetUpdatedAudit();
    }

    public string FullName => $"{Manufacturer} {Model} ({Year})";
    public bool IsAssignedToTeam => TeamId.HasValue;
}
