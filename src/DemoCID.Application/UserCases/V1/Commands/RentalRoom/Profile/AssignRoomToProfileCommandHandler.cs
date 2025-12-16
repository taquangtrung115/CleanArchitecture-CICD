using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Profile;

public sealed class AssignRoomToProfileCommandHandler : ICommandHandler<ProfileCommand.AssignRoomToProfileCommand>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> _profileRepository;
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignRoomToProfileCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> profileRepository,
        IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> roomRepository,
        IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ProfileCommand.AssignRoomToProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.FindByIdAsync(request.ProfileId, cancellationToken);
        if (profile == null)
            return Result.Failure(new Error("Profile.NotFound", $"Profile with ID {request.ProfileId} not found"));

        var room = await _roomRepository.FindByIdAsync(request.RoomId, cancellationToken);
        if (room == null)
            return Result.Failure(new Error("Room.NotFound", $"Room with ID {request.RoomId} not found"));

        profile.AssignRoom(request.RoomId, request.RentStartDate, request.DepositAmount);
        room.SetAvailability(false);

        _profileRepository.Update(profile);
        _roomRepository.Update(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
