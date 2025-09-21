using System;
using System.ComponentModel.DataAnnotations;

namespace DemoCICD.Domain.Entities.Identity;

public class PasswordResetToken
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string ResetCode { get; set; } = string.Empty;
    
    [Required]
    public DateTime ExpiresAt { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    public bool IsUsed { get; set; }
    
    public DateTime? UsedAt { get; set; }
    
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    
    public bool IsValid => !IsUsed && !IsExpired;
}