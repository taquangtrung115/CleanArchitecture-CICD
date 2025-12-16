using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Room;

/// <summary>
/// Command Handler c?p nh?t tr?ng thái phòng
/// </summary>
public sealed class UpdateRoomAvailabilityCommandHandler : ICommandHandler<RoomCommand.UpdateRoomAvailabilityCommand>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoomAvailabilityCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> roomRepository,
        IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RoomCommand.UpdateRoomAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (room == null)
            return Result.Failure(new Error("Room.NotFound", $"Room with ID {request.Id} not found"));

        room.SetAvailability(request.IsAvailable);
        
        _roomRepository.Update(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
