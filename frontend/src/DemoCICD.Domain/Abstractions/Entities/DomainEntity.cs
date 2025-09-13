

namespace DemoCICD.Domain.Abstractions.Entities;

public abstract class DomainEntity<T>
{
    public virtual T Id { get; set; }

    /// <summary>
    /// True if domain entity has an identity
    /// </summary>
    /// <returns></returns>
    public bool IsTransient()
    {
        return Id.Equals(default(T));
    }
}
