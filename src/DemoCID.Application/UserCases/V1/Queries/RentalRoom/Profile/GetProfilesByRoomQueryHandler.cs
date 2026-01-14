using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Profile;

public sealed class GetProfilesByRoomQueryHandler : IQueryHandler<ProfileQuery.GetProfilesByRoomQuery, List<ProfileResponse.Response>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IMapper _mapper;

    public GetProfilesByRoomQueryHandler(IProfileRepository profileRepository, IMapper mapper)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<ProfileResponse.Response>>> Handle(ProfileQuery.GetProfilesByRoomQuery request, CancellationToken cancellationToken)
    {
        var profiles = await _profileRepository.GetProfilesByRoomAsync(request.RoomId);

        // Use AutoMapper to map
        var responses = _mapper.Map<List<ProfileResponse.Response>>(profiles);

        return Result.Success(responses);
    }
}
