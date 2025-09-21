

namespace DemoCICD.Domain.Entities.Identity;

public class ActionInFunction
{
    public string ActionId { get; set; }
    public string FunctionId { get; set; }
    
    public virtual Action Action { get; set; }
    public virtual Function Function { get; set; }
}
