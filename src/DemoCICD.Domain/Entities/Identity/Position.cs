using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoCICD.Domain.Entities.Identity;

public class Position
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
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
}