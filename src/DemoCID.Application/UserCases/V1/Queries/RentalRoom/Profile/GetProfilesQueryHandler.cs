using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Reponsitories;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Profile;

public sealed class GetProfilesQueryHandler : IQueryHandler<ProfileQuery.GetProfilesQuery, PagedResult<ProfileResponse.Response>>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> _profileRepository;
    private readonly IMapper _mapper;

    public GetProfilesQueryHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> profileRepository,
        IMapper mapper)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<ProfileResponse.Response>>> Handle(ProfileQuery.GetProfilesQuery request, CancellationToken cancellationToken)
    {
        var query = _profileRepository.FindAll();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(p => p.FullName.Contains(request.SearchTerm) || 
                                    p.PhoneNumber.Contains(request.SearchTerm) ||
                                    p.IdentityCard.Contains(request.SearchTerm));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == request.IsActive.Value);
        }

        if (request.RoomId.HasValue)
        {
            query = query.Where(p => p.RoomId == request.RoomId.Value);
        }

        query = ApplySorting(query, request.SortColumn, request.SortOrder);

        var totalCount = await query.CountAsync(cancellationToken);
        var profiles = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // Use AutoMapper to map
        var responses = _mapper.Map<List<ProfileResponse.Response>>(profiles);

        var pagedResult = PagedResult<ProfileResponse.Response>.Create(responses, request.PageIndex, request.PageSize, totalCount);
        return Result.Success(pagedResult);
    }

    private static IQueryable<Domain.Entities.RentalRoom.Profiles.Profile> ApplySorting(
        IQueryable<Domain.Entities.RentalRoom.Profiles.Profile> query,
        string? sortColumn,
        Contract.Enumerations.SortOrder? sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortColumn))
            return query.OrderBy(p => p.FullName);

        var isDescending = sortOrder == Contract.Enumerations.SortOrder.Descending;

        return sortColumn.ToLower() switch
        {
            "fullname" => isDescending ? query.OrderByDescending(p => p.FullName) : query.OrderBy(p => p.FullName),
            "phonenumber" => isDescending ? query.OrderByDescending(p => p.PhoneNumber) : query.OrderBy(p => p.PhoneNumber),
            "rentstartdate" => isDescending ? query.OrderByDescending(p => p.RentStartDate) : query.OrderBy(p => p.RentStartDate),
            "createdat" => isDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
            _ => query.OrderBy(p => p.FullName)
        };
    }
}
