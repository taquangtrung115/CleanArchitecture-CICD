using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Race;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Race;

public sealed class GetRaceByIdQueryHandler : IQueryHandler<Query.GetRaceByIdQuery, Response.RaceResponse>
{
    private readonly IRaceRepository _raceRepository;
    private readonly IMapper _mapper;

    public GetRaceByIdQueryHandler(IRaceRepository raceRepository, IMapper mapper)
    {
        _raceRepository = raceRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.RaceResponse>> Handle(Query.GetRaceByIdQuery request, CancellationToken cancellationToken)
    {
        var race = await _raceRepository.FindByIdAsync(request.Id, cancellationToken);

        if (race == null)
        {
            return Result.Failure<Response.RaceResponse>(
                Error.NotFound("Race.NotFound", $"Race with ID {request.Id} was not found"));
        }

        var response = _mapper.Map<Response.RaceResponse>(race);
        return Result.Success(response);
    }
}