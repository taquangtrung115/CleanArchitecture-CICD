using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;
using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.RentalRoom.Bills;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Bill;

public sealed class AddBillDetailCommandHandler : ICommandHandler<BillCommand.AddBillDetailCommand, Guid>
{
    private readonly IBillRepository _billRepository;
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> _billRepositoryBase;
    private readonly IRepositoryBase<BillDetail, Guid> _billDetailRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddBillDetailCommandHandler(
        IBillRepository billRepository,
        IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> billRepositoryBase,
        IRepositoryBase<BillDetail, Guid> billDetailRepository,
        IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _billRepositoryBase = billRepositoryBase;
        _billDetailRepository = billDetailRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(BillCommand.AddBillDetailCommand request, CancellationToken cancellationToken)
    {
        // Use specific repository method to get bill with details
        var bill = await _billRepository.GetBillWithDetailsAsync(request.BillId);
        
        if (bill == null)
            return Result.Failure<Guid>(new Error("Bill.NotFound", $"Bill with ID {request.BillId} not found"));

        var billDetail = new BillDetail(
            Guid.NewGuid(),
            request.BillId,
            request.ServiceType,
            request.ServiceName,
            request.Unit,
            request.Quantity,
            request.UnitPrice,
            request.OldIndex,
            request.NewIndex,
            request.Notes);

        bill.AddBillDetail(billDetail);
        
        _billDetailRepository.Add(billDetail);
        _billRepositoryBase.Update(bill);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(billDetail.Id);
    }
}
