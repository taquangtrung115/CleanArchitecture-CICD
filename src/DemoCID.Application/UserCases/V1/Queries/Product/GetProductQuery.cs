using DemoCID.Application.Abstractions.Message;

namespace DemoCID.Application.UserCases.V1.Queries.Product;

public sealed class GetProductQuery : IQuery<GetProductResponse>
{
    public string Name { get; set; }
}
