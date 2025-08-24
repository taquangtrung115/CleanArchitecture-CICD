
using DemoCICD.Domain.Shared;
using DemoCICD.Application.Abstractions.Message;

namespace DemoCICD.Application.UserCases.V1.Queries.Product;

public sealed class GetProductQueryHandler : IQueryHandler<GetProductQuery, GetProductResponse>
{
    public Task<Result<GetProductResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
