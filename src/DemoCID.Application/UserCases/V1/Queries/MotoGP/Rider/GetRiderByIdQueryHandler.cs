using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Rider;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Rider;

public sealed class GetRiderByIdQueryHandler : IQueryHandler<Query.GetRiderByIdQuery, Response.RiderResponse>
{
    private readonly IRiderRepository _riderRepository;
    private readonly IMapper _mapper;

    public GetRiderByIdQueryHandler(IRiderRepository riderRepository, IMapper mapper)
    {
        _riderRepository = riderRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.RiderResponse>> Handle(Query.GetRiderByIdQuery request, CancellationToken cancellationToken)
    {
        var rider = await _riderRepository.FindByIdAsync(request.Id, cancellationToken);
        if (rider == null)
        {
            return Result.Failure<Response.RiderResponse>(
                Error.NotFound("Rider.NotFound", $"Rider with ID {request.Id} was not found"));
        }

        var riderResponse = _mapper.Map<Response.RiderResponse>(rider);
        return Result.Success(riderResponse);
    }
}