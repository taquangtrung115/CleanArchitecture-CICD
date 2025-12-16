using System;
using DemoCICD.Contract.Enumerations;
using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.RentalRoom.Bills;

/// <summary>
/// Entity quản lý chi tiết hóa đơn (các dịch vụ: điện, nước, internet, ...)
/// </summary>
public class BillDetail : AuditableEntity<Guid>
{
    /// <summary>
    /// ID hóa đơn
    /// </summary>
    public Guid BillId { get; private set; }

    /// <summary>
    /// Loại dịch vụ (Điện, Nước, Internet, Rác, Vệ sinh, Xe, ...)
    /// </summary>
    public ServiceType ServiceType { get; private set; }

    /// <summary>
    /// Tên dịch vụ
    /// </summary>
    public string ServiceName { get; private set; }

    /// <summary>
    /// Đơn vị tính (kWh, m³, tháng, xe, ...)
    /// </summary>
    public string Unit { get; private set; }

    /// <summary>
    /// Chỉ số cũ (dùng cho điện, nước)
    /// </summary>
    public decimal? OldIndex { get; private set; }

    /// <summary>
    /// Chỉ số mới (dùng cho điện, nước)
    /// </summary>
    public decimal? NewIndex { get; private set; }

    /// <summary>
    /// Số lượng sử dụng (chỉ số mới - chỉ số cũ, hoặc số lượng cố định)
    /// </summary>
    public decimal Quantity { get; private set; }

    /// <summary>
    /// Đơn giá
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Thành tiền (Số lượng × Đơn giá)
    /// </summary>
    public decimal TotalPrice { get; private set; }

    /// <summary>
    /// Ghi chú thêm
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation property - must be virtual for lazy loading
    /// <summary>
    /// Hóa đơn (lazy loading)
    /// </summary>
    public virtual Bill? Bill { get; private set; }

    // Protected parameterless constructor for EF Core proxies
    protected BillDetail()
    {
        ServiceName = string.Empty;
        Unit = string.Empty;
    }

    public BillDetail(
        Guid id,
        Guid billId,
        ServiceType serviceType,
        string serviceName,
        string unit,
        decimal quantity,
        decimal unitPrice,
        decimal? oldIndex = null,
        decimal? newIndex = null,
        string? notes = null)
    {
        Id = id;
        BillId = billId;
        ServiceType = serviceType;
        ServiceName = serviceName;
        Unit = unit;
        OldIndex = oldIndex;
        NewIndex = newIndex;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = quantity * unitPrice;
        Notes = notes;
    }

    /// <summary>
    /// Cập nhật chỉ số điện nước
    /// </summary>
    public void UpdateIndex(decimal oldIndex, decimal newIndex)
    {
        OldIndex = oldIndex;
        NewIndex = newIndex;
        Quantity = newIndex - oldIndex;
        TotalPrice = Quantity * UnitPrice;
    }

    /// <summary>
    /// Cập nhật số lượng và đơn giá
    /// </summary>
    public void UpdateQuantityAndPrice(decimal quantity, decimal unitPrice)
    {
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = quantity * unitPrice;
    }

    /// <summary>
    /// Cập nhật ghi chú
    /// </summary>
    public void UpdateNotes(string notes)
    {
        Notes = notes;
    }
}
