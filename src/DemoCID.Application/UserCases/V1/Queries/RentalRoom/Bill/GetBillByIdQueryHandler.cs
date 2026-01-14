using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Bill;

public sealed class GetBillByIdQueryHandler : IQueryHandler<BillQuery.GetBillByIdQuery, BillResponse.DetailedResponse>
{
    private readonly IBillRepository _billRepository;
    private readonly IMapper _mapper;

    public GetBillByIdQueryHandler(IBillRepository billRepository, IMapper mapper)
    {
        _billRepository = billRepository;
        _mapper = mapper;
    }

    public async Task<Result<BillResponse.DetailedResponse>> Handle(BillQuery.GetBillByIdQuery request, CancellationToken cancellationToken)
    {
        // Use specific repository method to get bill with details (Include BillDetails, Room, Profile)
        var bill = await _billRepository.GetBillWithDetailsAsync(request.Id);

        if (bill == null)
            return Result.Failure<BillResponse.DetailedResponse>(
                new Error("Bill.NotFound", $"Bill with ID {request.Id} not found"));

        // Use AutoMapper to map Bill to DetailedResponse (includes BillDetails mapping)
        var response = _mapper.Map<BillResponse.DetailedResponse>(bill);

        return Result.Success(response);
    }
}
