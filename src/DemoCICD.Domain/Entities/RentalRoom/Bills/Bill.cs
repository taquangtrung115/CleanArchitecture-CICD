using System;
using System.Collections.Generic;
using System.Linq;
using DemoCICD.Contract.Enumerations;
using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.RentalRoom.Bills;

/// <summary>
/// Entity quản lý hóa đơn hàng tháng của phòng trọ
/// </summary>
public class Bill : AuditableEntity<Guid>
{
    /// <summary>
    /// Mã hóa đơn (Ví dụ: HD202401001)
    /// </summary>
    public string BillNumber { get; private set; }

    /// <summary>
    /// ID phòng trọ
    /// </summary>
    public Guid RoomId { get; private set; }

    /// <summary>
    /// ID người thuê
    /// </summary>
    public Guid ProfileId { get; private set; }

    /// <summary>
    /// Tháng của hóa đơn (1-12)
    /// </summary>
    public int Month { get; private set; }

    /// <summary>
    /// Năm của hóa đơn
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// Ngày phát hành hóa đơn
    /// </summary>
    public DateTime IssueDate { get; private set; }

    /// <summary>
    /// Ngày hết hạn thanh toán
    /// </summary>
    public DateTime DueDate { get; private set; }

    /// <summary>
    /// Tiền phòng cơ bản
    /// </summary>
    public decimal RoomPrice { get; private set; }

    /// <summary>
    /// Tổng tiền dịch vụ (điện, nước, internet, ...)
    /// </summary>
    public decimal ServiceTotal { get; private set; }

    /// <summary>
    /// Tổng tiền phải thanh toán
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Số tiền đã thanh toán
    /// </summary>
    public decimal PaidAmount { get; private set; }

    /// <summary>
    /// Số tiền còn nợ
    /// </summary>
    public decimal RemainingAmount { get; private set; }

    /// <summary>
    /// Trạng thái thanh toán: Pending (Chưa thanh toán), PartiallyPaid (Thanh toán 1 phần), Paid (Đã thanh toán), Overdue (Quá hạn)
    /// </summary>
    public BillStatus Status { get; private set; }

    /// <summary>
    /// Ngày thanh toán
    /// </summary>
    public DateTime? PaymentDate { get; private set; }

    /// <summary>
    /// Phương thức thanh toán (Tiền mặt, Chuyển khoản, ...)
    /// </summary>
    public string? PaymentMethod { get; private set; }

    /// <summary>
    /// Ghi chú thêm
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties - MUST be virtual for lazy loading
    /// <summary>
    /// Danh sách chi tiết hóa đơn (lazy loading)
    /// </summary>
    public virtual ICollection<BillDetail> BillDetails { get; private set; }

    /// <summary>
    /// Phòng trọ (lazy loading)
    /// </summary>
    public virtual Rooms.Room? Room { get; private set; }

    /// <summary>
    /// Người thuê (lazy loading)
    /// </summary>
    public virtual Profiles.Profile? Profile { get; private set; }

    // Protected parameterless constructor for EF Core proxies
    protected Bill()
    {
        BillNumber = string.Empty;
        BillDetails = new List<BillDetail>();
    }

    public Bill(
        Guid id,
        string billNumber,
        Guid roomId,
        Guid profileId,
        int month,
        int year,
        DateTime issueDate,
        DateTime dueDate,
        decimal roomPrice)
    {
        Id = id;
        BillNumber = billNumber;
        RoomId = roomId;
        ProfileId = profileId;
        Month = month;
        Year = year;
        IssueDate = issueDate;
        DueDate = dueDate;
        RoomPrice = roomPrice;
        ServiceTotal = 0;
        TotalAmount = roomPrice;
        PaidAmount = 0;
        RemainingAmount = roomPrice;
        Status = BillStatus.Pending;
        BillDetails = new List<BillDetail>();
    }

    /// <summary>
    /// Thêm chi tiết hóa đơn (dịch vụ)
    /// </summary>
    public void AddBillDetail(BillDetail billDetail)
    {
        BillDetails.Add(billDetail);
        RecalculateTotal();
    }

    /// <summary>
    /// Xóa chi tiết hóa đơn
    /// </summary>
    public void RemoveBillDetail(Guid billDetailId)
    {
        var detail = BillDetails.FirstOrDefault(d => d.Id == billDetailId);
        if (detail != null)
        {
            BillDetails.Remove(detail);
            RecalculateTotal();
        }
    }

    /// <summary>
    /// Tính lại tổng tiền
    /// </summary>
    private void RecalculateTotal()
    {
        ServiceTotal = BillDetails.Sum(d => d.TotalPrice);
        TotalAmount = RoomPrice + ServiceTotal;
        RemainingAmount = TotalAmount - PaidAmount;
    }

    /// <summary>
    /// Thanh toán hóa đơn (toàn bộ hoặc 1 phần)
    /// </summary>
    public void MakePayment(decimal amount, string paymentMethod, DateTime? paymentDate = null)
    {
        PaidAmount += amount;
        RemainingAmount = TotalAmount - PaidAmount;
        PaymentMethod = paymentMethod;
        PaymentDate = paymentDate ?? DateTime.UtcNow;

        if (RemainingAmount <= 0)
        {
            Status = BillStatus.Paid;
            RemainingAmount = 0;
        }
        else
        {
            Status = BillStatus.PartiallyPaid;
        }
    }

    /// <summary>
    /// Đánh dấu hóa đơn quá hạn
    /// </summary>
    public void MarkAsOverdue()
    {
        if (Status != BillStatus.Paid && DateTime.UtcNow > DueDate)
        {
            Status = BillStatus.Overdue;
        }
    }

    /// <summary>
    /// Cập nhật ghi chú
    /// </summary>
    public void UpdateNotes(string notes)
    {
        Notes = notes;
    }

    /// <summary>
    /// Cập nhật ngày hết hạn
    /// </summary>
    public void UpdateDueDate(DateTime dueDate)
    {
        DueDate = dueDate;
    }
}
