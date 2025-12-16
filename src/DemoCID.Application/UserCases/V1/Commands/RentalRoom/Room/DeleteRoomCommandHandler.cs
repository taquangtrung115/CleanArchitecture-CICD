using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Room;

/// <summary>
/// Command Handler xóa phòng
/// </summary>
public sealed class DeleteRoomCommandHandler : ICommandHandler<RoomCommand.DeleteRoomCommand>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> roomRepository,
        IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RoomCommand.DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (room == null)
            return Result.Failure(new Error("Room.NotFound", $"Room with ID {request.Id} not found"));

        _roomRepository.Remove(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
