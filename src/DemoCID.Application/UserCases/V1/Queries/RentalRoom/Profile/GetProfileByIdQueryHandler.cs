using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Profile;

public sealed class GetProfileByIdQueryHandler : IQueryHandler<ProfileQuery.GetProfileByIdQuery, ProfileResponse.Response>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> _profileRepository;
    private readonly IMapper _mapper;

    public GetProfileByIdQueryHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> profileRepository,
        IMapper mapper)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProfileResponse.Response>> Handle(ProfileQuery.GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.FindByIdAsync(request.Id, cancellationToken);

        if (profile == null)
            return Result.Failure<ProfileResponse.Response>(
                new Error("Profile.NotFound", $"Profile with ID {request.Id} not found"));

        // Use AutoMapper to map
        var response = _mapper.Map<ProfileResponse.Response>(profile);

        return Result.Success(response);
    }
}
