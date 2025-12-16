using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Bill;

public sealed class MakePaymentCommandHandler : ICommandHandler<BillCommand.MakePaymentCommand>
{
    private readonly IBillRepository _billRepository;
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> _billRepositoryBase;
    private readonly IUnitOfWork _unitOfWork;

    public MakePaymentCommandHandler(
        IBillRepository billRepository,
        IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> billRepositoryBase,
        IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _billRepositoryBase = billRepositoryBase;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(BillCommand.MakePaymentCommand request, CancellationToken cancellationToken)
    {
        // Use specific repository method to get bill with details
        var bill = await _billRepository.GetBillWithDetailsAsync(request.BillId);
        
        if (bill == null)
            return Result.Failure(new Error("Bill.NotFound", $"Bill with ID {request.BillId} not found"));

        bill.MakePayment(request.Amount, request.PaymentMethod, request.PaymentDate);
        
        _billRepositoryBase.Update(bill);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
