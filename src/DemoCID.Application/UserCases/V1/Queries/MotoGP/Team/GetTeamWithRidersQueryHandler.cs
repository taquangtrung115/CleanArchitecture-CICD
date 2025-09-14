using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Team;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Team;

public sealed class GetTeamWithRidersQueryHandler : IQueryHandler<Query.GetTeamWithRidersQuery, Response.TeamWithRidersResponse>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public GetTeamWithRidersQueryHandler(ITeamRepository teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.TeamWithRidersResponse>> Handle(Query.GetTeamWithRidersQuery request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.FindByIdAsync(request.Id, cancellationToken);
        if (team == null)
        {
            return Result.Failure<Response.TeamWithRidersResponse>(
                Error.NotFound("Team.NotFound", $"Team with ID {request.Id} was not found"));
        }

        // Map team to response including riders
        var teamResponse = _mapper.Map<Response.TeamWithRidersResponse>(team);
        return Result.Success(teamResponse);
    }
}