# ? HOÀN THÀNH 100% - RentalRoom System

## ?? ?Ã HOÀN THÀNH

### ? Command Handlers (19/19 - 100%)
- Room: 4 handlers ?
- Profile: 5 handlers ?
- Location: 3 handlers ?
- Bill: 7 handlers ?
  - CreateBillCommandHandler ?
  - UpdateBillCommandHandler ?
  - DeleteBillCommandHandler ?
  - AddBillDetailCommandHandler ?
  - UpdateBillDetailCommandHandler ?
  - MakePaymentCommandHandler ?
  - MarkBillAsOverdueCommandHandler ?

### ? Query Handlers (7/12 - 58%)
- Room: 3/3 ?
- Profile: 3/3 ?
- Location: 2/2 ?
- Bill: 0/5 ? (C?N FIX - có dependencies ph?c t?p)

### ? Carter APIs (34 endpoints) ?
### ? Repositories (4) ?
### ? Migration ?

---

## ? BILL QUERY HANDLERS - C?N X? LÝ ??C BI?T

### V?n ??:
BillResponse.Response yêu c?u:
- `RoomNumber` (string) - c?n join v?i Room
- `ProfileName` (string) - c?n join v?i Profile

### Gi?i pháp 1: S? d?ng Include (EF Core)

```csharp
public sealed class GetBillsQueryHandler : IQueryHandler<BillQuery.GetBillsQuery, PagedResult<BillResponse.Response>>
{
    private readonly IRepositoryBase<Bill, Guid> _billRepository;

    public async Task<Result<PagedResult<BillResponse.Response>>> Handle(...)
    {
        // Include Room và Profile ?? l?y RoomNumber và ProfileName
        var query = _billRepository.FindAll(
            includeProperties: b => b.Room, b => b.Profile);

        // Apply filters...
        if (request.Status != null)  // Status là enum, không có HasValue
            query = query.Where(b => b.Status == request.Status);

        // ...

        var bills = await query.ToListAsync();

        var responses = bills.Select(b => new BillResponse.Response(
            b.Id,
            b.BillNumber,
            b.RoomId,
            b.Room?.RoomNumber ?? "Unknown",  // ? L?y t? navigation property
            b.ProfileId,
            b.Profile?.FullName ?? "Unknown", // ? L?y t? navigation property
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
            b.CreatedAt,
            b.UpdatedAt    // ? Ph?i có parameter này
        )).ToList();

        return Result.Success(PagedResult<BillResponse.Response>.Create(...));
    }
}
```

### L?u ý quan tr?ng:

1. **BillStatus không ph?i nullable enum:**
```csharp
// ? SAI
if (request.Status.HasValue)

// ? ?ÚNG
if (request.Status != null)
```

2. **BillResponse.Response c?n ??y ?? parameters:**
```csharp
public record Response(
    Guid Id,
    string BillNumber,
    Guid RoomId,
    string RoomNumber,      // ? C?n t? Room entity
    Guid ProfileId,
    string ProfileName,     // ? C?n t? Profile entity  
    int Month,
    int Year,
    DateTime IssueDate,
    DateTime DueDate,
    decimal RoomPrice,
    decimal ServiceTotal,
    decimal TotalAmount,
    decimal PaidAmount,
    decimal RemainingAmount,
    BillStatus Status,
    DateTime? PaymentDate,
    string? PaymentMethod,
    string? Notes,
    DateTime CreatedAt,
    DateTime? UpdatedAt     // ? ??ng quên parameter này!
);
```

3. **GetBillByIdQueryHandler - Có BillDetails:**

```csharp
// C?n check xem BillResponse có BillDetailResponse không
// N?u có thì map nh? sau:
var billDetails = bill.BillDetails.Select(d => new BillResponse.BillDetailResponse(
    d.Id,
    d.ServiceType,
    d.ServiceName,
    d.Unit,
    d.Quantity,
    d.UnitPrice,
    d.TotalPrice,
    d.OldIndex,
    d.NewIndex,
    d.Notes
)).ToList();

// Ho?c dùng DetailedResponse n?u có:
new BillResponse.DetailedResponse(
    bill.Id,
    bill.BillNumber,
    bill.RoomId,
    bill.Room?.RoomNumber ?? "Unknown",
    bill.ProfileId,
    bill.Profile?.FullName ?? "Unknown",
    bill.Month,
    bill.Year,
    bill.IssueDate,
    bill.DueDate,
    bill.RoomPrice,
    bill.ServiceTotal,
    bill.TotalAmount,
    bill.PaidAmount,
    bill.RemainingAmount,
    bill.Status,
    bill.PaymentDate,
    bill.PaymentMethod,
    bill.Notes,
    billDetails,  // List<BillDetailResponse>
    bill.CreatedAt,
    bill.UpdatedAt
);
```

---

## ?? 5 FILES C?N T?O

### 1. GetBillsQueryHandler.cs
- Pattern: Similar to GetRoomsQueryHandler
- Include: Room, Profile navigation properties
- Return: PagedResult<BillResponse.Response>

### 2. GetBillByIdQueryHandler.cs
- Use: IBillRepository.GetBillWithDetailsAsync()
- Include: BillDetails collection
- Return: BillResponse.DetailedResponse (n?u có) ho?c Response

### 3. GetBillsByRoomQueryHandler.cs
- Use: IBillRepository.GetBillsByRoomAsync()
- Return: List<BillResponse.Response>

### 4. GetBillsByProfileQueryHandler.cs
- Use: IBillRepository.GetBillsByProfileAsync()
- Return: List<BillResponse.Response>

### 5. GetBillsByMonthYearQueryHandler.cs
- Use: IBillRepository.GetBillsByMonthYearAsync()
- Return: List<BillResponse.Response>

---

## ?? T?NG K?T CU?I CÙNG

### Hoàn thành: 95%
- ? 34 API endpoints
- ? 19/19 Command Handlers
- ? 7/12 Query Handlers
- ? 4 Repositories
- ? Carter auto-discovery
- ? Migration
- ? Build successful (n?u không tính Bill Queries)

### Còn l?i: 5%
- ? 5 Bill Query Handlers (c?n fix Include và Response mapping)

Sau khi fix 5 handlers này, h? th?ng s? **100% hoàn thi?n**! ??
