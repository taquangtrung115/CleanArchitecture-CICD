using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Team;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Team;

public sealed class GetTeamsQueryHandler : IQueryHandler<Query.GetTeamsQuery, PagedResult<Response.TeamResponse>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public GetTeamsQueryHandler(ITeamRepository teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<Response.TeamResponse>>> Handle(Query.GetTeamsQuery request, CancellationToken cancellationToken)
    {
        var query = _teamRepository.FindAll();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(t => t.Name.Contains(request.SearchTerm) ||
                                   t.ShortName.Contains(request.SearchTerm) ||
                                   t.Description!.Contains(request.SearchTerm));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(t => t.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.CountryCode))
        {
            query = query.Where(t => t.Country.Code == request.CountryCode);
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(request.SortColumn))
        {
            query = request.SortColumn.ToLower() switch
            {
                "name" => request.SortOrder == Contract.Enumerations.SortOrder.Descending
                    ? query.OrderByDescending(t => t.Name)
                    : query.OrderBy(t => t.Name),
                "shortname" => request.SortOrder == Contract.Enumerations.SortOrder.Descending
                    ? query.OrderByDescending(t => t.ShortName)
                    : query.OrderBy(t => t.ShortName),
                "foundedyear" => request.SortOrder == Contract.Enumerations.SortOrder.Descending
                    ? query.OrderByDescending(t => t.FoundedYear)
                    : query.OrderBy(t => t.FoundedYear),
                _ => query.OrderBy(t => t.Name)
            };
        }
        else
        {
            query = query.OrderBy(t => t.Name);
        }

        var teams = await PagedResult<Domain.Entities.MotoGP.TeamRiderManagement.Team>.CreateAsync(
            query, request.PageIndex, request.PageSize);

        var teamResponses = _mapper.Map<PagedResult<Response.TeamResponse>>(teams);

        return Result.Success(teamResponses);
    }
}