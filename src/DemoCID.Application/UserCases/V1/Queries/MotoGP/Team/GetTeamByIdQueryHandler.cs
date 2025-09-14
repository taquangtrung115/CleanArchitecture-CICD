using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Team;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Team;

public sealed class GetTeamByIdQueryHandler : IQueryHandler<Query.GetTeamByIdQuery, Response.TeamResponse>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public GetTeamByIdQueryHandler(ITeamRepository teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.TeamResponse>> Handle(Query.GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.FindByIdAsync(request.Id, cancellationToken);
        if (team == null)
        {
            return Result.Failure<Response.TeamResponse>(
                Error.NotFound("Team.NotFound", $"Team with ID {request.Id} was not found"));
        }

        var teamResponse = _mapper.Map<Response.TeamResponse>(team);
        return Result.Success(teamResponse);
    }
}