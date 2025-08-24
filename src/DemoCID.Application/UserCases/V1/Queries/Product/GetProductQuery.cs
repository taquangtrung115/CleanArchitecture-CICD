using DemoCICD.Application.Abstractions.Message;

namespace DemoCICD.Application.UserCases.V1.Queries.Product;

public sealed class GetProductQuery : IQuery<GetProductResponse>
{
    public string Name { get; set; }
}
