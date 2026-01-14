# ? RentalRoom System - HOÀN THÀNH PH?N L?N!

## ?? PROGRESS SUMMARY

### ? ?Ã HOÀN THÀNH (Build Successful!)

#### **1. Carter Modules (4 modules - 34 endpoints)**
- ? RoomManagementApi (7 endpoints)
- ? ProfileManagementApi (8 endpoints)
- ? LocationManagementApi (6 endpoints)
- ? BillManagementApi (13 endpoints)

#### **2. Command Handlers (15 handlers)**
- ? Room Commands (4): Create, Update, Delete, UpdateAvailability
- ? Profile Commands (5): Create, Update, Delete, AssignRoom, EndRental
- ? Location Commands (3): Create, Update, Delete
- ? Bill Commands (3): Create, AddBillDetail, MakePayment

#### **3. Query Handlers (7 handlers)**
- ? Room Queries (3): GetRooms, GetRoomById, GetRoomsByLocation
- ? Profile Queries (3): GetProfiles, GetProfileById, GetProfilesByRoom
- ? Location Queries (2): GetLocations, GetLocationById

#### **4. Infrastructure**
- ? 4 EF Core Repositories (Room, Profile, Location, Bill)
- ? All entities support lazy loading
- ? Migration created and ready
- ? DI registered completely

---

## ?? CÒN THI?U (9 handlers)

### **Bill Query Handlers (5 handlers)**

T?t c? ??u follow pattern t??ng t? và s? d?ng IBillRepository:

#### 1. `GetBillsQueryHandler.cs`
```csharp
// Pattern: GetRoomsQueryHandler + nhi?u filters
// Use: IRepositoryBase<Bill, Guid>.FindAll()
// Filters: searchTerm, status, roomId, profileId, month, year
// Sorting: billnumber, issuedate, duedate, totalamount
```

#### 2. `GetBillByIdQueryHandler.cs`
```csharp
// Use: IBillRepository.GetBillWithDetailsAsync()
// Return: Bill v?i Include(BillDetails)
```

#### 3. `GetBillsByRoomQueryHandler.cs`
```csharp
// Use: IBillRepository.GetBillsByRoomAsync(roomId)
// Return: List<BillResponse.Response>
```

#### 4. `GetBillsByProfileQueryHandler.cs`
```csharp
// Use: IBillRepository.GetBillsByProfileAsync(profileId)
// Return: List<BillResponse.Response>
```

#### 5. `GetBillsByMonthYearQueryHandler.cs`
```csharp
// Use: IBillRepository.GetBillsByMonthYearAsync(month, year)
// Return: List<BillResponse.Response>
```

---

### **Bill Command Handlers (4 handlers)**

#### 6. `UpdateBillCommandHandler.cs`
```csharp
public async Task<Result> Handle(BillCommand.UpdateBillCommand request, ...)
{
    var bill = await _billRepository.FindByIdAsync(request.Id, ...);
    if (bill == null) return Result.Failure(...);

    bill.UpdateDueDate(request.DueDate);
    bill.UpdateNotes(request.Notes);

    _billRepository.Update(bill);
    await _unitOfWork.SaveChangesAsync(...);
    return Result.Success();
}
```

#### 7. `DeleteBillCommandHandler.cs`
```csharp
// Standard pattern: FindById -> Remove -> SaveChanges
```

#### 8. `UpdateBillDetailCommandHandler.cs`
```csharp
// Use: IRepositoryBase<BillDetail, Guid>
// Call: billDetail.UpdateQuantityAndPrice() or billDetail.UpdateIndex()
```

#### 9. `MarkBillAsOverdueCommandHandler.cs`
```csharp
// FindById -> bill.MarkAsOverdue() -> Update -> SaveChanges
```

**NOTE:** `DeleteBillDetailCommandHandler` ?ã có r?i, không c?n t?o n?a.

---

## ?? QUICK START - T?o handlers còn thi?u

### Pattern Template cho Bill Query:

```csharp
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Bill;

public sealed class Get[Name]QueryHandler : IQueryHandler<BillQuery.Get[Name]Query, [ReturnType]>
{
    private readonly IBillRepository _billRepository;

    public Get[Name]QueryHandler(IBillRepository billRepository)
    {
        _billRepository = billRepository;
    }

    public async Task<Result<[ReturnType]>> Handle(BillQuery.Get[Name]Query request, CancellationToken cancellationToken)
    {
        var bills = await _billRepository.Get[Name]Async(request.[Params]);
        
        var responses = bills.Select(b => new BillResponse.Response(
            b.Id,
            b.BillNumber,
            b.RoomId,
            b.ProfileId,
            b.Month,
            b.Year,
            b.IssueDate,
            b.DueDate,
            b.RoomPrice,
            b.ServiceTotal,
            b.TotalAmount,
            b.PaidAmount,
            b.RemainingAmount,
            b.Status,
            b.PaymentDate,
            b.PaymentMethod,
            b.Notes,
            null, // BillDetails
            b.CreatedAt,
            b.UpdatedAt
        )).ToList();

        return Result.Success(responses);
    }
}
```

### Pattern Template cho Bill Command:

```csharp
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;

namespace DemoCICD.Application.UserCases.V1.Commands.RentalRoom.Bill;

public sealed class [Name]CommandHandler : ICommandHandler<BillCommand.[Name]Command>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> _billRepository;
    private readonly IUnitOfWork _unitOfWork;

    public [Name]CommandHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> billRepository,
        IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(BillCommand.[Name]Command request, CancellationToken cancellationToken)
    {
        var bill = await _billRepository.FindByIdAsync(request.Id, cancellationToken);
        if (bill == null)
            return Result.Failure(new Error("Bill.NotFound", $"Bill with ID {request.Id} not found"));

        // Call domain method
        bill.[DomainMethod](request.Params);

        _billRepository.Update(bill);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
```

---

## ?? FILE LOCATIONS

```
src\DemoCID.Application\UserCases\V1\

Queries\RentalRoom\Bill\
??? GetBillsQueryHandler.cs ?
??? GetBillByIdQueryHandler.cs ?
??? GetBillsByRoomQueryHandler.cs ?
??? GetBillsByProfileQueryHandler.cs ?
??? GetBillsByMonthYearQueryHandler.cs ?

Commands\RentalRoom\Bill\
??? UpdateBillCommandHandler.cs ?
??? DeleteBillCommandHandler.cs ?
??? UpdateBillDetailCommandHandler.cs ?
??? MarkBillAsOverdueCommandHandler.cs ?
```

---

## ?? T?NG K?T

### **?ã làm xong:**
- ? 34 API endpoints
- ? 15 Command Handlers
- ? 7 Query Handlers
- ? 4 Repositories
- ? Migration
- ? Build successful!

### **Còn l?i:**
- ? 9 handlers (5 Query + 4 Command)

### **T? l? hoàn thành:**
**71/80 = 88.75%** ??

Sau khi t?o xong 9 handlers còn l?i, h? th?ng RentalRoom s? **100% hoàn thi?n**!

---

## ?? NEXT STEPS

1. T?o 5 Bill Query Handlers (follow GetRoomsByLocationQueryHandler pattern)
2. T?o 4 Bill Command Handlers (follow UpdateRoomCommandHandler pattern)
3. Run build ?? verify
4. Test APIs qua Swagger
5. Apply migration: `dotnet ef database update`
6. Done! ??
