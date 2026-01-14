using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Location;

public sealed class GetLocationByIdQueryHandler : IQueryHandler<LocationQuery.GetLocationByIdQuery, LocationResponse.Response>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Locations.Location, Guid> _locationRepository;
    private readonly IMapper _mapper;

    public GetLocationByIdQueryHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Locations.Location, Guid> locationRepository,
        IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    public async Task<Result<LocationResponse.Response>> Handle(LocationQuery.GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var location = await _locationRepository.FindByIdAsync(request.Id, cancellationToken);

        if (location == null)
            return Result.Failure<LocationResponse.Response>(
                new Error("Location.NotFound", $"Location with ID {request.Id} not found"));

        // Use AutoMapper to map
        var response = _mapper.Map<LocationResponse.Response>(location);

        return Result.Success(response);
    }
}
