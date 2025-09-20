using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Race;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Race;

public sealed class GetRaceWithResultsQueryHandler : IQueryHandler<Query.GetRaceWithResultsQuery, Response.RaceWithResultsResponse>
{
    private readonly IRaceRepository _raceRepository;
    private readonly IMapper _mapper;

    public GetRaceWithResultsQueryHandler(IRaceRepository raceRepository, IMapper mapper)
    {
        _raceRepository = raceRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.RaceWithResultsResponse>> Handle(Query.GetRaceWithResultsQuery request, CancellationToken cancellationToken)
    {
        var race = await _raceRepository.GetRaceWithEntriesAsync(request.Id, cancellationToken);

        if (race == null)
        {
            return Result.Failure<Response.RaceWithResultsResponse>(
                Error.NotFound("Race.NotFound", $"Race with ID {request.Id} was not found"));
        }

        var response = _mapper.Map<Response.RaceWithResultsResponse>(race);
        return Result.Success(response);
    }
}