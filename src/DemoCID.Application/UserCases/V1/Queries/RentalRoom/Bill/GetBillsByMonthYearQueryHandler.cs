using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Bill;

public sealed class GetBillsByMonthYearQueryHandler : IQueryHandler<BillQuery.GetBillsByMonthYearQuery, List<BillResponse.Response>>
{
    private readonly IBillRepository _billRepository;
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> _roomRepository;
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> _profileRepository;
    private readonly IMapper _mapper;

    public GetBillsByMonthYearQueryHandler(
        IBillRepository billRepository,
        IRepositoryBase<Domain.Entities.RentalRoom.Rooms.Room, Guid> roomRepository,
        IRepositoryBase<Domain.Entities.RentalRoom.Profiles.Profile, Guid> profileRepository,
        IMapper mapper)
    {
        _billRepository = billRepository;
        _roomRepository = roomRepository;
        _profileRepository = profileRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<BillResponse.Response>>> Handle(BillQuery.GetBillsByMonthYearQuery request, CancellationToken cancellationToken)
    {
        var bills = await _billRepository.GetBillsByMonthYearAsync(request.Month, request.Year);

        // Get Room and Profile info separately to avoid N+1 problem
        var roomIds = bills.Select(b => b.RoomId).Distinct().ToList();
        var profileIds = bills.Select(b => b.ProfileId).Distinct().ToList();

        var rooms = await Task.WhenAll(roomIds.Select(id => _roomRepository.FindByIdAsync(id, cancellationToken)));
        var profiles = await Task.WhenAll(profileIds.Select(id => _profileRepository.FindByIdAsync(id, cancellationToken)));

        // Set navigation properties for AutoMapper
        var roomDict = rooms.Where(r => r != null).ToDictionary(r => r!.Id);
        var profileDict = profiles.Where(p => p != null).ToDictionary(p => p!.Id);

        foreach (var bill in bills)
        {
            if (roomDict.TryGetValue(bill.RoomId, out var room))
                bill.GetType().GetProperty("Room")?.SetValue(bill, room);
            
            if (profileDict.TryGetValue(bill.ProfileId, out var profile))
                bill.GetType().GetProperty("Profile")?.SetValue(bill, profile);
        }

        // Use AutoMapper to map
        var responses = _mapper.Map<List<BillResponse.Response>>(bills);

        return Result.Success(responses);
    }
}
