using System;
using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.RentalRoom.Locations;

/// <summary>
/// Entity qu?n lý ??a ch? c?a phòng tr?
/// </summary>
public class Location : AuditableEntity<Guid>
{
    /// <summary>
    /// S? nhà, tên ???ng
    /// </summary>
    public string Street { get; private set; }

    /// <summary>
    /// Ph??ng/Xã
    /// </summary>
    public string Ward { get; private set; }

    /// <summary>
    /// Qu?n/Huy?n
    /// </summary>
    public string District { get; private set; }

    /// <summary>
    /// T?nh/Thành ph?
    /// </summary>
    public string City { get; private set; }

    /// <summary>
    /// ??a ch? ??y ?? (chu?i ghép t? các thông tin trên)
    /// </summary>
    public string FullAddress { get; private set; }

    /// <summary>
    /// Mã b?u ?i?n
    /// </summary>
    public string? PostalCode { get; private set; }

    /// <summary>
    /// T?a ?? v? ?? (Latitude) - dùng cho b?n ??
    /// </summary>
    public double? Latitude { get; private set; }

    /// <summary>
    /// T?a ?? kinh ?? (Longitude) - dùng cho b?n ??
    /// </summary>
    public double? Longitude { get; private set; }

    /// <summary>
    /// Ghi chú thêm v? ??a ch? (ví d?: g?n ngã t?, c?nh siêu th?, ...)
    /// </summary>
    public string? Notes { get; private set; }

    // Protected parameterless constructor for EF Core proxies
    protected Location()
    {
        Street = string.Empty;
        Ward = string.Empty;
        District = string.Empty;
        City = string.Empty;
        FullAddress = string.Empty;
    }

    public Location(
        Guid id,
        string street,
        string ward,
        string district,
        string city,
        string? postalCode = null,
        double? latitude = null,
        double? longitude = null,
        string? notes = null)
    {
        Id = id;
        Street = street;
        Ward = ward;
        District = district;
        City = city;
        PostalCode = postalCode;
        Latitude = latitude;
        Longitude = longitude;
        Notes = notes;
        FullAddress = BuildFullAddress();
    }

    /// <summary>
    /// C?p nh?t thông tin ??a ch?
    /// </summary>
    public void UpdateLocation(
        string street,
        string ward,
        string district,
        string city,
        string? postalCode = null,
        double? latitude = null,
        double? longitude = null,
        string? notes = null)
    {
        Street = street;
        Ward = ward;
        District = district;
        City = city;
        PostalCode = postalCode;
        Latitude = latitude;
        Longitude = longitude;
        Notes = notes;
        FullAddress = BuildFullAddress();
    }

    /// <summary>
    /// C?p nh?t t?a ?? GPS
    /// </summary>
    public void UpdateCoordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Xây d?ng ??a ch? ??y ?? t? các thành ph?n
    /// </summary>
    private string BuildFullAddress()
    {
        return $"{Street}, {Ward}, {District}, {City}";
    }
}
