
using Microsoft.AspNetCore.Identity;

namespace DemoCICD.Domain.Entities.Identity;

public class AppRole : IdentityRole<Guid>
{
    public Guid RoleId { get; set; }
    public string Description { get; set; }
    public string RoleCode { get; set; }
    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
    public virtual ICollection<Permission> Permissions { get; set; }
}
