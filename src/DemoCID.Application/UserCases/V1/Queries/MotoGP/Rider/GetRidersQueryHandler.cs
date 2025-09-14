using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Rider;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Rider;

public sealed class GetRidersQueryHandler : IQueryHandler<Query.GetRidersQuery, PagedResult<Response.RiderResponse>>
{
    private readonly IRiderRepository _riderRepository;
    private readonly IMapper _mapper;

    public GetRidersQueryHandler(IRiderRepository riderRepository, IMapper mapper)
    {
        _riderRepository = riderRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<Response.RiderResponse>>> Handle(Query.GetRidersQuery request, CancellationToken cancellationToken)
    {
        var query = _riderRepository.FindAll();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(r => r.FirstName.Contains(request.SearchTerm) ||
                                   r.LastName.Contains(request.SearchTerm) ||
                                   r.Nickname!.Contains(request.SearchTerm));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(r => r.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.CountryCode))
        {
            query = query.Where(r => r.Nationality.Code == request.CountryCode);
        }

        if (request.TeamId.HasValue)
        {
            query = query.Where(r => r.CurrentTeamId == request.TeamId.Value);
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(request.SortColumn))
        {
            query = request.SortColumn.ToLower() switch
            {
                "firstname" => request.SortOrder == Contract.Enumerations.SortOrder.Descending
                    ? query.OrderByDescending(r => r.FirstName)
                    : query.OrderBy(r => r.FirstName),
                "lastname" => request.SortOrder == Contract.Enumerations.SortOrder.Descending
                    ? query.OrderByDescending(r => r.LastName)
                    : query.OrderBy(r => r.LastName),
                "racingnumber" => request.SortOrder == Contract.Enumerations.SortOrder.Descending
                    ? query.OrderByDescending(r => r.RacingNumber)
                    : query.OrderBy(r => r.RacingNumber),
                "dateofbirth" => request.SortOrder == Contract.Enumerations.SortOrder.Descending
                    ? query.OrderByDescending(r => r.DateOfBirth)
                    : query.OrderBy(r => r.DateOfBirth),
                _ => query.OrderBy(r => r.LastName).ThenBy(r => r.FirstName)
            };
        }
        else
        {
            query = query.OrderBy(r => r.LastName).ThenBy(r => r.FirstName);
        }

        var riders = await PagedResult<Domain.Entities.MotoGP.TeamRiderManagement.Rider>.CreateAsync(
            query, request.PageIndex, request.PageSize);

        var riderResponses = _mapper.Map<PagedResult<Response.RiderResponse>>(riders);

        return Result.Success(riderResponses);
    }
}