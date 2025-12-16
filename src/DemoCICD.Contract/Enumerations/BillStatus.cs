using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SmartEnum;

namespace DemoCICD.Contract.Enumerations;


/// <summary>
/// Enum trạng thái thanh toán hóa đơn
/// </summary>
public class BillStatus : SmartEnum<BillStatus>
{
    public BillStatus(string name, int value)
        : base(name, value)
    {
    }
    /// <summary>
    /// Chưa thanh toán
    /// </summary>
    public static readonly BillStatus Pending = new(nameof(Pending), 1);
    /// <summary>
    /// Thanh toán 1 phần
    /// </summary>
    public static readonly BillStatus PartiallyPaid = new(nameof(PartiallyPaid), 2);
    /// <summary>
    /// Đã thanh toán
    /// </summary>
    public static readonly BillStatus Paid = new(nameof(Paid), 3);
    /// <summary>
    /// Quá hạn
    /// </summary>
    public static readonly BillStatus Overdue = new(nameof(Overdue), 4);

    public static implicit operator BillStatus(string name)
        => FromName(name);

    public static implicit operator BillStatus(int value)
        => FromValue(value);

    public static implicit operator string(BillStatus status)
        => status.Name;

    public static implicit operator int(BillStatus status)
        => status.Value;
}
