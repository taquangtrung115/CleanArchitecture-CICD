
using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V2.Product;
using DemoCICD.Domain.Abstractions.Dappers;

namespace DemoCICD.Application.UserCases.V2.Queries.Product;

public sealed class GetProductsQueryHandler : IQueryHandler<Query.GetProductsQuery, Result<List<Response.ProductResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Result<List<Response.ProductResponse>>>> Handle(Query.GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Products.GetAllAsync();

        var results = _mapper.Map<List<Response.ProductResponse>>(products);

        return Result.Success(results);
    }
}
