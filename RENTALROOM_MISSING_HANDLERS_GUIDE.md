# RentalRoom Missing Handlers Implementation Guide

## ? HOÀN THÀNH - Build successful!

### ?ã fix:
- ? T?t c? accessibility issues (private -> public records)
- ? Build thành công - No errors

---

## ?? HANDLERS CÒN THI?U C?N T?O

### 1. Room Query Handlers (1 handler)

#### `GetRoomsByLocationQueryHandler.cs`
**Location:** `src\DemoCID.Application\UserCases\V1\Queries\RentalRoom\Room\`

```csharp
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Room;

public sealed class GetRoomsByLocationQueryHandler : IQueryHandler<RoomQuery.GetRoomsByLocationQuery, List<RoomResponse.Response>>
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomsByLocationQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Result<List<RoomResponse.Response>>> Handle(RoomQuery.GetRoomsByLocationQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetRoomsByLocationAsync(request.LocationId);

        var responses = rooms.Select(r => new RoomResponse.Response(
            r.Id,
            r.RoomNumber,
            r.Capacity,
            r.PricePerNight,
            r.Description,
            r.IsAvailable,
            r.LocationId,
            null,
            r.CreatedAt,
            r.UpdatedAt
        )).ToList();

        return Result.Success(responses);
    }
}
```

---

### 2. Profile Query Handlers (3 handlers)

#### `GetProfilesQueryHandler.cs`
**Pattern t??ng t?:** GetRoomsQueryHandler (s? d?ng IRepositoryBase.FindAll())

#### `GetProfileByIdQueryHandler.cs`
**Pattern t??ng t?:** GetRoomByIdQueryHandler (s? d?ng IRepositoryBase.FindByIdAsync())

#### `GetProfilesByRoomQueryHandler.cs`
**S? d?ng:** IProfileRepository.GetProfilesByRoomAsync()

---

### 3. Location Query Handlers (2 handlers)

#### `GetLocationsQueryHandler.cs`
**Pattern t??ng t?:** GetRoomsQueryHandler

#### `GetLocationByIdQueryHandler.cs`
**Pattern t??ng t?:** GetRoomByIdQueryHandler

---

### 4. Bill Query Handlers (5 handlers)

#### `GetBillsQueryHandler.cs`
**Pattern t??ng t?:** GetRoomsQueryHandler v?i nhi?u filters h?n

#### `GetBillByIdQueryHandler.cs`
**S? d?ng:** IBillRepository.GetBillWithDetailsAsync() (có Include BillDetails)

#### `GetBillsByRoomQueryHandler.cs`
**S? d?ng:** IBillRepository.GetBillsByRoomAsync()

#### `GetBillsByProfileQueryHandler.cs`
**S? d?ng:** IBillRepository.GetBillsByProfileAsync()

#### `GetBillsByMonthYearQueryHandler.cs`
**S? d?ng:** IBillRepository.GetBillsByMonthYearAsync()

---

### 5. Bill Command Handlers (5 handlers)

#### `UpdateBillCommandHandler.cs`
```csharp
public sealed class UpdateBillCommandHandler : ICommandHandler<BillCommand.UpdateBillCommand>
{
    private readonly IRepositoryBase<Bill, Guid> _billRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result> Handle(BillCommand.UpdateBillCommand request, CancellationToken cancellationToken)
    {
        var bill = await _billRepository.FindByIdAsync(request.Id, cancellationToken);
        if (bill == null) return Result.Failure(...);

        bill.UpdateDueDate(request.DueDate);
        bill.UpdateNotes(request.Notes);

        _billRepository.Update(bill);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
```

#### `DeleteBillCommandHandler.cs`
**Pattern:** Standard delete (FindById -> Remove -> SaveChanges)

#### `UpdateBillDetailCommandHandler.cs`
```csharp
public async Task<Result> Handle(BillCommand.UpdateBillDetailCommand request, CancellationToken cancellationToken)
{
    var billDetail = await _billDetailRepository.FindByIdAsync(request.Id, cancellationToken);
    if (billDetail == null) return Result.Failure(...);

    billDetail.UpdateQuantityAndPrice(request.Quantity, request.UnitPrice);
    if (request.OldIndex.HasValue && request.NewIndex.HasValue)
        billDetail.UpdateIndex(request.OldIndex.Value, request.NewIndex.Value);

    _billDetailRepository.Update(billDetail);
    await _unitOfWork.SaveChangesAsync(cancellationToken);
    return Result.Success();
}
```

#### `DeleteBillDetailCommandHandler.cs`
**S? d?ng:** IBillRepository.GetBillWithDetailsAsync() -> bill.RemoveBillDetail()

#### `MarkBillAsOverdueCommandHandler.cs`
```csharp
public async Task<Result> Handle(BillCommand.MarkBillAsOverdueCommand request, CancellationToken cancellationToken)
{
    var bill = await _billRepository.FindByIdAsync(request.BillId, cancellationToken);
    if (bill == null) return Result.Failure(...);

    bill.MarkAsOverdue();

    _billRepository.Update(bill);
    await _unitOfWork.SaveChangesAsync(cancellationToken);
    return Result.Success();
}
```

---

## ?? PATTERNS TÓM T?T

### Query Handler Pattern:
```csharp
// Simple Query
IRepositoryBase<TEntity, Guid>.FindByIdAsync()
IRepositoryBase<TEntity, Guid>.FindAll().Where(...).ToListAsync()

// Complex Query with Include
ISpecificRepository.GetEntityWithDetailsAsync()
```

### Command Handler Pattern:
```csharp
1. FindById from Repository
2. Validate entity exists
3. Call domain method on entity
4. Update repository
5. SaveChanges via IUnitOfWork
```

---

## ?? FILE STRUCTURE

```
src\DemoCID.Application\UserCases\V1\
??? Queries\RentalRoom\
?   ??? Room\
?   ?   ??? GetRoomsQueryHandler.cs ?
?   ?   ??? GetRoomByIdQueryHandler.cs ?
?   ?   ??? GetRoomsByLocationQueryHandler.cs ?
?   ??? Profile\
?   ?   ??? GetProfilesQueryHandler.cs ?
?   ?   ??? GetProfileByIdQueryHandler.cs ?
?   ?   ??? GetProfilesByRoomQueryHandler.cs ?
?   ??? Location\
?   ?   ??? GetLocationsQueryHandler.cs ?
?   ?   ??? GetLocationByIdQueryHandler.cs ?
?   ??? Bill\
?       ??? GetBillsQueryHandler.cs ?
?       ??? GetBillByIdQueryHandler.cs ?
?       ??? GetBillsByRoomQueryHandler.cs ?
?       ??? GetBillsByProfileQueryHandler.cs ?
?       ??? GetBillsByMonthYearQueryHandler.cs ?
?
??? Commands\RentalRoom\
    ??? Bill\
        ??? UpdateBillCommandHandler.cs ?
        ??? DeleteBillCommandHandler.cs ?
        ??? UpdateBillDetailCommandHandler.cs ?
        ??? DeleteBillDetailCommandHandler.cs ?
        ??? MarkBillAsOverdueCommandHandler.cs ?
```

---

## ? T?NG K?T

**?ã hoàn thành:**
- ? 4 Carter Modules (34 endpoints)
- ? 15 Command Handlers
- ? 2 Query Handlers
- ? 4 EF Core Repositories
- ? Migration created
- ? Build successful

**Còn c?n t?o:**
- ? 11 Query Handlers
- ? 5 Bill Command Handlers

**T?ng c?ng còn:** 16 handlers

Sau khi t?o xong 16 handlers này, h? th?ng RentalRoom s? hoàn thi?n 100%!
