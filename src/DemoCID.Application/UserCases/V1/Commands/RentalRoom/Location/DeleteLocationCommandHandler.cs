using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Location;

public sealed class DeleteLocationCommandHandler : ICommandHandler<LocationCommand.DeleteLocationCommand>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Locations.Location, Guid> _locationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLocationCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Locations.Location, Guid> locationRepository,
        IUnitOfWork unitOfWork)
    {
        _locationRepository = locationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LocationCommand.DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        var location = await _locationRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (location == null)
            return Result.Failure(new Error("Location.NotFound", $"Location with ID {request.Id} not found"));

        _locationRepository.Remove(location);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
