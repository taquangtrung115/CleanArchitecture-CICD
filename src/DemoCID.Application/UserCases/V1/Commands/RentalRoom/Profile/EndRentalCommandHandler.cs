using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Profile;

public sealed class EndRentalCommandHandler : ICommandHandler<ProfileCommand.EndRentalCommand>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> _profileRepository;
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EndRentalCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> profileRepository,
        IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> roomRepository,
        IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ProfileCommand.EndRentalCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.FindByIdAsync(request.ProfileId, cancellationToken);
        if (profile == null)
            return Result.Failure(new Error("Profile.NotFound", $"Profile with ID {request.ProfileId} not found"));

        if (profile.RoomId.HasValue)
        {
            var room = await _roomRepository.FindByIdAsync(profile.RoomId.Value, cancellationToken);
            if (room != null)
            {
                room.SetAvailability(true);
                _roomRepository.Update(room);
            }
        }

        profile.EndRental(request.RentEndDate);
        _profileRepository.Update(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
