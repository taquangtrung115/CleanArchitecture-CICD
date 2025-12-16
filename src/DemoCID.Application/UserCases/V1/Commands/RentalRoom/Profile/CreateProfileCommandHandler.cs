using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Profile;

/// <summary>
/// Command Handler t?o profile ng??i thuê m?i
/// </summary>
public sealed class CreateProfileCommandHandler : ICommandHandler<ProfileCommand.CreateProfileCommand, Guid>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProfileCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> profileRepository,
        IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(ProfileCommand.CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = new Domain.Entities.RentalRoom.Profiles.Profile(
            Guid.NewGuid(),
            request.FullName,
            request.PhoneNumber,
            request.IdentityCard,
            request.Email,
            request.DateOfBirth,
            request.PermanentAddress,
            request.Occupation);

        _profileRepository.Add(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(profile.Id);
    }
}
