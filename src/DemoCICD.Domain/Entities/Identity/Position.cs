using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.Identity;

public class Position : DomainEntity<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;
    
    public int Level { get; set; } = 1;
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    [MaxLength(100)]
    public string? CreatedBy { get; set; }
    
    [MaxLength(100)]
    public string? UpdatedBy { get; set; }
    
    // Navigation properties
    public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();

    // Domain methods
    public bool HasAssignedUsers() => Users?.Any() == true;
    
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsHigherLevelThan(Position other)
    {
        return other != null && Level > other.Level;
    }

    public bool IsAtSameLevelAs(Position other)
    {
        return other != null && Level == other.Level;
    }
}