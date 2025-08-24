using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Contract.Services.Product;

public static class Response
{
    public record ProductResponse(Guid id, string name, decimal price, string description);
}
