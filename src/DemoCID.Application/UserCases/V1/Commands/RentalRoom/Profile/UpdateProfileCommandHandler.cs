using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Profile;

public sealed class UpdateProfileCommandHandler : ICommandHandler<ProfileCommand.UpdateProfileCommand>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> profileRepository,
        IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ProfileCommand.UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (profile == null)
            return Result.Failure(new Error("Profile.NotFound", $"Profile with ID {request.Id} not found"));

        profile.UpdatePersonalInfo(
            request.FullName,
            request.PhoneNumber,
            request.IdentityCard,
            request.Email,
            request.DateOfBirth,
            request.PermanentAddress,
            request.Occupation);

        _profileRepository.Update(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
