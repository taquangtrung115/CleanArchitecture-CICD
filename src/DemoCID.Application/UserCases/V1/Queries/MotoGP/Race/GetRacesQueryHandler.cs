using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Race;
using DemoCICD.Contract.Enumerations;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.RaceManagement;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Race;

public sealed class GetRacesQueryHandler : IQueryHandler<Query.GetRacesQuery, PagedResult<Response.RaceResponse>>
{
    private readonly IRaceRepository _raceRepository;
    private readonly IMapper _mapper;

    public GetRacesQueryHandler(IRaceRepository raceRepository, IMapper mapper)
    {
        _raceRepository = raceRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<Response.RaceResponse>>> Handle(Query.GetRacesQuery request, CancellationToken cancellationToken)
    {
        var query = _raceRepository.FindAll();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(r => r.Name.Contains(request.SearchTerm) ||
                                   r.CircuitName.Contains(request.SearchTerm) ||
                                   r.Country.Name.Contains(request.SearchTerm));
        }

        if (request.SeasonId.HasValue)
        {
            query = query.Where(r => r.SeasonId == request.SeasonId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (Enum.TryParse<RaceStatus>(request.Status, true, out var status))
            {
                query = query.Where(r => r.Status == status);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.CountryCode))
        {
            query = query.Where(r => r.Country.Code == request.CountryCode);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(r => r.RaceDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(r => r.RaceDate <= request.ToDate.Value);
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(request.SortColumn))
        {
            query = request.SortColumn.ToLower() switch
            {
                "name" => request.SortOrder == SortOrder.Descending
                                        ? query.OrderByDescending(r => r.Name)
                                        : query.OrderBy(r => r.Name),
                "racedate" => request.SortOrder == SortOrder.Descending
                                        ? query.OrderByDescending(r => r.RaceDate)
                                        : query.OrderBy(r => r.RaceDate),
                "roundnumber" => request.SortOrder == SortOrder.Descending
                                        ? query.OrderByDescending(r => r.RoundNumber)
                                        : query.OrderBy(r => r.RoundNumber),
                "countryname" => request.SortOrder == SortOrder.Descending
                                        ? query.OrderByDescending(r => r.Country.Name)
                                        : query.OrderBy(r => r.Country.Name),
                _ => query.OrderBy(r => r.RoundNumber),
            };
        }
        else
        {
            query = query.OrderBy(r => r.RoundNumber);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var races = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var racesResponse = _mapper.Map<IEnumerable<Response.RaceResponse>>(races);

        var pagedResult = PagedResult<Response.RaceResponse>.Create(
            racesResponse.ToList(), 
            request.PageIndex, 
            request.PageSize, 
            totalCount);

        return Result.Success(pagedResult);
    }
}
