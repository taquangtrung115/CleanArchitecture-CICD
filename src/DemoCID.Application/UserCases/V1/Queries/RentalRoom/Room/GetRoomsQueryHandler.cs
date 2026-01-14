using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Reponsitories;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Room;

public sealed class GetRoomsQueryHandler : IQueryHandler<RoomQuery.GetRoomsQuery, PagedResult<RoomResponse.Response>>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomsQueryHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> roomRepository,
        IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<RoomResponse.Response>>> Handle(RoomQuery.GetRoomsQuery request, CancellationToken cancellationToken)
    {
        var query = _roomRepository.FindAll();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(r => r.RoomNumber.Contains(request.SearchTerm) || 
                                    r.Description.Contains(request.SearchTerm));
        }

        if (request.IsAvailable.HasValue)
        {
            query = query.Where(r => r.IsAvailable == request.IsAvailable.Value);
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(r => r.PricePerNight >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(r => r.PricePerNight <= request.MaxPrice.Value);
        }

        if (request.MinCapacity.HasValue)
        {
            query = query.Where(r => r.Capacity >= request.MinCapacity.Value);
        }

        // Apply sorting
        query = ApplySorting(query, request.SortColumn, request.SortOrder);

        var totalCount = await query.CountAsync(cancellationToken);

        var rooms = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // Use AutoMapper to map
        var responses = _mapper.Map<List<RoomResponse.Response>>(rooms);

        var pagedResult = PagedResult<RoomResponse.Response>.Create(
            responses,
            request.PageIndex,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }

    private static IQueryable<Domain.Entities.RentalRoom.Rooms.Room> ApplySorting(
        IQueryable<Domain.Entities.RentalRoom.Rooms.Room> query,
        string? sortColumn,
        Contract.Enumerations.SortOrder? sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortColumn))
            return query.OrderBy(r => r.RoomNumber);

        var isDescending = sortOrder == Contract.Enumerations.SortOrder.Descending;

        return sortColumn.ToLower() switch
        {
            "roomnumber" => isDescending ? query.OrderByDescending(r => r.RoomNumber) : query.OrderBy(r => r.RoomNumber),
            "capacity" => isDescending ? query.OrderByDescending(r => r.Capacity) : query.OrderBy(r => r.Capacity),
            "pricepernight" => isDescending ? query.OrderByDescending(r => r.PricePerNight) : query.OrderBy(r => r.PricePerNight),
            "createdat" => isDescending ? query.OrderByDescending(r => r.CreatedAt) : query.OrderBy(r => r.CreatedAt),
            _ => query.OrderBy(r => r.RoomNumber)
        };
    }
}
