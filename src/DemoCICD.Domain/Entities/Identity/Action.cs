using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Domain.Entities.Identity;

public class Action
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int? SortOrder { get; set; }
    public bool? IsActive { get; set; }
    public virtual ICollection<Permission> Permissions { get; set; }
    public virtual ICollection<ActionInFunction> ActionInFunctions { get; set; }
}
