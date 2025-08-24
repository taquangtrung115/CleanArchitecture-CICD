using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;
using static DemoCICD.Contract.Services.V2.Product.Response;

namespace DemoCICD.Contract.Services.V2.Product;

public static class Query
{
    public record GetProductsQuery() : IQuery<Result<List<ProductResponse>>>;
    public record GetProductByIdQuery(Guid Id) : IQuery<ProductResponse>;
}
