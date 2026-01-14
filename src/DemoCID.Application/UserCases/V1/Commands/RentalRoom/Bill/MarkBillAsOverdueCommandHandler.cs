using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Bill;

public sealed class MarkBillAsOverdueCommandHandler : ICommandHandler<BillCommand.MarkBillAsOverdueCommand>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> _billRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkBillAsOverdueCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> billRepository,
        IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(BillCommand.MarkBillAsOverdueCommand request, CancellationToken cancellationToken)
    {
        var bill = await _billRepository.FindByIdAsync(request.BillId, cancellationToken);
        
        if (bill == null)
            return Result.Failure(new Error("Bill.NotFound", $"Bill with ID {request.BillId} not found"));

        bill.MarkAsOverdue();

        _billRepository.Update(bill);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
