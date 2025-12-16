using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SmartEnum;

namespace DemoCICD.Contract.Enumerations;

/// <summary>
/// Enum loại dịch vụ
/// </summary>
public class ServiceType : SmartEnum<ServiceType>
{
    public ServiceType(string name, int value)
        : base(name, value)
    {
    }
    /// <summary>
    /// Tiền điện
    /// </summary>
    public static readonly ServiceType Electricity = new(nameof(Electricity), 1);
    /// <summary>
    /// Tiền nước
    /// </summary>
    public static readonly ServiceType Water = new(nameof(Water), 2);
    /// <summary>
    /// Tiền Internet/Wifi
    /// </summary>
    public static readonly ServiceType Internet = new(nameof(Internet), 3);
    /// <summary>
    /// Tiền rác
    /// </summary>
    public static readonly ServiceType Garbage = new(nameof(Garbage), 4);
    /// <summary>
    /// Tiền vệ sinh chung
    /// </summary>
    public static readonly ServiceType Cleaning = new(nameof(Cleaning), 5);
    /// <summary>
    /// Tiền gửi xe
    /// </summary>
    public static readonly ServiceType Parking = new(nameof(Parking), 6);
    /// <summary>
    /// Tiền bảo trì, sửa chữa
    /// </summary>
    public static readonly ServiceType Maintenance = new(nameof(Maintenance), 7);
    /// <summary>
    /// Dịch vụ khác
    /// </summary>
    public static readonly ServiceType Other = new(nameof(Other), 99);

    public static implicit operator ServiceType(string name)
        => FromName(name);

    public static implicit operator ServiceType(int value)
        => FromValue(value);

    public static implicit operator string(ServiceType status)
        => status.Name;

    public static implicit operator int(ServiceType status)
        => status.Value;
}

