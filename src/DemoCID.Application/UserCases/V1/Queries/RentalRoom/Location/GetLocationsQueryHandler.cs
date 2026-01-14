using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Reponsitories;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Location;

public sealed class GetLocationsQueryHandler : IQueryHandler<LocationQuery.GetLocationsQuery, PagedResult<LocationResponse.Response>>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Locations.Location, Guid> _locationRepository;
    private readonly IMapper _mapper;

    public GetLocationsQueryHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Locations.Location, Guid> locationRepository,
        IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<LocationResponse.Response>>> Handle(LocationQuery.GetLocationsQuery request, CancellationToken cancellationToken)
    {
        var query = _locationRepository.FindAll();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(l => l.FullAddress.Contains(request.SearchTerm) ||
                                    l.Street.Contains(request.SearchTerm) ||
                                    l.Ward.Contains(request.SearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            query = query.Where(l => l.City == request.City);
        }

        if (!string.IsNullOrWhiteSpace(request.District))
        {
            query = query.Where(l => l.District == request.District);
        }

        query = ApplySorting(query, request.SortColumn, request.SortOrder);

        var totalCount = await query.CountAsync(cancellationToken);
        var locations = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // Use AutoMapper to map
        var responses = _mapper.Map<List<LocationResponse.Response>>(locations);

        var pagedResult = PagedResult<LocationResponse.Response>.Create(responses, request.PageIndex, request.PageSize, totalCount);
        return Result.Success(pagedResult);
    }

    private static IQueryable<Domain.Entities.RentalRoom.Locations.Location> ApplySorting(
        IQueryable<Domain.Entities.RentalRoom.Locations.Location> query,
        string? sortColumn,
        Contract.Enumerations.SortOrder? sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortColumn))
            return query.OrderBy(l => l.City).ThenBy(l => l.District);

        var isDescending = sortOrder == Contract.Enumerations.SortOrder.Descending;

        return sortColumn.ToLower() switch
        {
            "city" => isDescending ? query.OrderByDescending(l => l.City) : query.OrderBy(l => l.City),
            "district" => isDescending ? query.OrderByDescending(l => l.District) : query.OrderBy(l => l.District),
            "ward" => isDescending ? query.OrderByDescending(l => l.Ward) : query.OrderBy(l => l.Ward),
            "createdat" => isDescending ? query.OrderByDescending(l => l.CreatedAt) : query.OrderBy(l => l.CreatedAt),
            _ => query.OrderBy(l => l.City).ThenBy(l => l.District)
        };
    }
}
