using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.RentalRoom.Bills;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Bill;

public sealed class UpdateBillDetailCommandHandler : ICommandHandler<BillCommand.UpdateBillDetailCommand>
{
    private readonly IRepositoryBase<BillDetail, Guid> _billDetailRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBillDetailCommandHandler(
        IRepositoryBase<BillDetail, Guid> billDetailRepository,
        IUnitOfWork unitOfWork)
    {
        _billDetailRepository = billDetailRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(BillCommand.UpdateBillDetailCommand request, CancellationToken cancellationToken)
    {
        var billDetail = await _billDetailRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (billDetail == null)
            return Result.Failure(new Error("BillDetail.NotFound", $"BillDetail with ID {request.Id} not found"));

        // Update quantity and price
        if (request.Quantity > 0 && request.UnitPrice > 0)
        {
            billDetail.UpdateQuantityAndPrice(request.Quantity, request.UnitPrice);
        }

        // Update index if provided (for electricity, water)
        if (request.OldIndex.HasValue && request.NewIndex.HasValue)
        {
            billDetail.UpdateIndex(request.OldIndex.Value, request.NewIndex.Value);
        }

        // Update notes
        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            billDetail.UpdateNotes(request.Notes);
        }

        _billDetailRepository.Update(billDetail);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
