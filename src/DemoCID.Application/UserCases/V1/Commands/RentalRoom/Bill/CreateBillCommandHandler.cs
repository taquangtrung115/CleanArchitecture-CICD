using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Bill;

public sealed class CreateBillCommandHandler : ICommandHandler<BillCommand.CreateBillCommand, Guid>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> _billRepository;
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> _roomRepository;
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBillCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> billRepository,
        IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> roomRepository,
        IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> profileRepository,
        IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _roomRepository = roomRepository;
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(BillCommand.CreateBillCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.FindByIdAsync(request.RoomId, cancellationToken);
        if (room == null)
            return Result.Failure<Guid>(new Error("Room.NotFound", $"Room with ID {request.RoomId} not found"));

        var profile = await _profileRepository.FindByIdAsync(request.ProfileId, cancellationToken);
        if (profile == null)
            return Result.Failure<Guid>(new Error("Profile.NotFound", $"Profile with ID {request.ProfileId} not found"));

        var bill = new Domain.Entities.RentalRoom.Bills.Bill(
            Guid.NewGuid(),
            request.BillNumber,
            request.RoomId,
            request.ProfileId,
            request.Month,
            request.Year,
            request.IssueDate,
            request.DueDate,
            request.RoomPrice);

        _billRepository.Add(bill);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(bill.Id);
    }
}
