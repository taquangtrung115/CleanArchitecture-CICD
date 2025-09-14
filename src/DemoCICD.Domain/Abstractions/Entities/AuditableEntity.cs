using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Abstractions.Entities;

public abstract class AuditableEntity<T> : DomainEntity<T>
{
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public string? UpdatedBy { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }

    protected AuditableEntity()
    {
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void SetCreatedAudit(string? createdBy = null)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void SetUpdatedAudit(string? updatedBy = null)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void SetDeletedAudit(string? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }

    public void RestoreEntity()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }
}