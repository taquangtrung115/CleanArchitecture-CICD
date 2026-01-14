using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Bill;

public sealed class UpdateBillCommandHandler : ICommandHandler<BillCommand.UpdateBillCommand>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> _billRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBillCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> billRepository,
        IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(BillCommand.UpdateBillCommand request, CancellationToken cancellationToken)
    {
        var bill = await _billRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (bill == null)
            return Result.Failure(new Error("Bill.NotFound", $"Bill with ID {request.Id} not found"));

        bill.UpdateDueDate(request.DueDate);
        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            bill.UpdateNotes(request.Notes);
        }

        _billRepository.Update(bill);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
