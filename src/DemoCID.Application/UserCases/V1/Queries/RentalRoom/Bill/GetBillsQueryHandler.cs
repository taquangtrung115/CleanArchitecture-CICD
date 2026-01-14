using AutoMapper;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Domain.Abstractions.Reponsitories;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Application.UserCases.V1.Queries.RentalRoom.Bill;

public sealed class GetBillsQueryHandler : IQueryHandler<BillQuery.GetBillsQuery, PagedResult<BillResponse.Response>>
{
    private readonly IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> _billRepository;
    private readonly IMapper _mapper;

    public GetBillsQueryHandler(
        IRepositoryBase<Domain.Entities.RentalRoom.Bills.Bill, Guid> billRepository,
        IMapper mapper)
    {
        _billRepository = billRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<BillResponse.Response>>> Handle(BillQuery.GetBillsQuery request, CancellationToken cancellationToken)
    {
        // Use FindAll with Include to load Room and Profile navigation properties
        var query = _billRepository.FindAll(null, b => b.Room, b => b.Profile);

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(b => b.BillNumber.Contains(request.SearchTerm) ||
                                    (b.Notes != null && b.Notes.Contains(request.SearchTerm)));
        }

        // BillStatus is enum, not nullable - check for null not HasValue
        if (request.Status != null)
        {
            query = query.Where(b => b.Status == request.Status);
        }

        if (request.RoomId.HasValue)
        {
            query = query.Where(b => b.RoomId == request.RoomId.Value);
        }

        if (request.ProfileId.HasValue)
        {
            query = query.Where(b => b.ProfileId == request.ProfileId.Value);
        }

        if (request.Month.HasValue)
        {
            query = query.Where(b => b.Month == request.Month.Value);
        }

        if (request.Year.HasValue)
        {
            query = query.Where(b => b.Year == request.Year.Value);
        }

        // Apply sorting
        query = ApplySorting(query, request.SortColumn, request.SortOrder);

        var totalCount = await query.CountAsync(cancellationToken);

        var bills = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // Use AutoMapper to map List<Bill> to List<BillResponse.Response>
        var responses = _mapper.Map<List<BillResponse.Response>>(bills);

        var pagedResult = PagedResult<BillResponse.Response>.Create(
            responses,
            request.PageIndex,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }

    private static IQueryable<Domain.Entities.RentalRoom.Bills.Bill> ApplySorting(
        IQueryable<Domain.Entities.RentalRoom.Bills.Bill> query,
        string? sortColumn,
        Contract.Enumerations.SortOrder? sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortColumn))
            return query.OrderByDescending(b => b.IssueDate);

        var isDescending = sortOrder == Contract.Enumerations.SortOrder.Descending;

        return sortColumn.ToLower() switch
        {
            "billnumber" => isDescending ? query.OrderByDescending(b => b.BillNumber) : query.OrderBy(b => b.BillNumber),
            "issuedate" => isDescending ? query.OrderByDescending(b => b.IssueDate) : query.OrderBy(b => b.IssueDate),
            "duedate" => isDescending ? query.OrderByDescending(b => b.DueDate) : query.OrderBy(b => b.DueDate),
            "totalamount" => isDescending ? query.OrderByDescending(b => b.TotalAmount) : query.OrderBy(b => b.TotalAmount),
            "status" => isDescending ? query.OrderByDescending(b => b.Status) : query.OrderBy(b => b.Status),
            _ => query.OrderByDescending(b => b.IssueDate)
        };
    }
}
