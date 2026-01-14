using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Bill;

public sealed class DeleteBillCommandHandler : ICommandHandler<BillCommand.DeleteBillCommand>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> _billRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBillCommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> billRepository,
        IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(BillCommand.DeleteBillCommand request, CancellationToken cancellationToken)
    {
        var bill = await _billRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (bill == null)
            return Result.Failure(new Error("Bill.NotFound", $"Bill with ID {request.Id} not found"));

        _billRepository.Remove(bill);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
