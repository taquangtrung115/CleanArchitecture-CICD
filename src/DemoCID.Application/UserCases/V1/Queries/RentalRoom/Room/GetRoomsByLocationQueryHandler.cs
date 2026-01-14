using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Room;

public sealed class GetRoomsByLocationQueryHandler : IQueryHandler<RoomQuery.GetRoomsByLocationQuery, List<RoomResponse.Response>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomsByLocationQueryHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<RoomResponse.Response>>> Handle(RoomQuery.GetRoomsByLocationQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetRoomsByLocationAsync(request.LocationId);

        // Use AutoMapper to map
        var responses = _mapper.Map<List<RoomResponse.Response>>(rooms);

        return Result.Success(responses);
    }
}
