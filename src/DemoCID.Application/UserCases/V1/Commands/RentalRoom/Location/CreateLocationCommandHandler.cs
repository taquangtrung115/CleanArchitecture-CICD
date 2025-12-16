using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Location;

public sealed class CreateLocationCommandHandler : ICommandHandler<LocationCommand.CreateLocationCommand, Guid>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Locations.Location, Guid> _locationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLocationCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Locations.Location, Guid> locationRepository,
        IUnitOfWork unitOfWork)
    {
        _locationRepository = locationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(LocationCommand.CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var location = new Domain.Entities.RentalRoom.Locations.Location(
            Guid.NewGuid(),
            request.Street,
            request.Ward,
            request.District,
            request.City,
            request.PostalCode,
            request.Latitude,
            request.Longitude,
            request.Notes);

        _locationRepository.Add(location);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(location.Id);
    }
}
