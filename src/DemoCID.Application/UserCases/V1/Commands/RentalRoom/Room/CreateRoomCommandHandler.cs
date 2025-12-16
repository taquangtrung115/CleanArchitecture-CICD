using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Room;

/// <summary>
/// Command Handler t?o phòng m?i
/// </summary>
public sealed class CreateRoomCommandHandler : ICommandHandler<RoomCommand.CreateRoomCommand, Guid>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> roomRepository,
        IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RoomCommand.CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Domain.Entities.RentalRoom.Rooms.Room(
            Guid.NewGuid(),
            request.RoomNumber,
            request.Capacity,
            request.PricePerNight,
            request.Description,
            request.LocationId);

        _roomRepository.Add(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(room.Id);
    }
}
