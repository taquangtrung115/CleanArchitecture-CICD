using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Room;

public sealed class GetRoomByIdQueryHandler : IQueryHandler<RoomQuery.GetRoomByIdQuery, RoomResponse.Response>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomByIdQueryHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> roomRepository,
        IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<Result<RoomResponse.Response>> Handle(RoomQuery.GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.FindByIdAsync(request.Id, cancellationToken);

        if (room == null)
            return Result.Failure<RoomResponse.Response>(
                new Error("Room.NotFound", $"Room with ID {request.Id} not found"));

        // Use AutoMapper to map
        var response = _mapper.Map<RoomResponse.Response>(room);

        return Result.Success(response);
    }
}
