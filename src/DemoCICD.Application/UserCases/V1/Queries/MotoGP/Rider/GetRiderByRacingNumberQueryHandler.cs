using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Rider;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Rider;

public sealed class GetRiderByRacingNumberQueryHandler : IQueryHandler<Query.GetRiderByRacingNumberQuery, Response.RiderResponse>
{
    private readonly IRiderRepository _riderRepository;
    private readonly IMapper _mapper;

    public GetRiderByRacingNumberQueryHandler(IRiderRepository riderRepository, IMapper mapper)
    {
        _riderRepository = riderRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.RiderResponse>> Handle(Query.GetRiderByRacingNumberQuery request, CancellationToken cancellationToken)
    {
        var rider = await _riderRepository.GetRiderByRacingNumberAsync(request.RacingNumber, cancellationToken);
        if (rider == null)
        {
            return Result.Failure<Response.RiderResponse>(
                Error.NotFound("Rider.NotFound", $"Rider with racing number {request.RacingNumber} was not found"));
        }

        var riderResponse = _mapper.Map<Response.RiderResponse>(rider);
        return Result.Success(riderResponse);
    }
}