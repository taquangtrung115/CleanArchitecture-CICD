using System;
using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.RentalRoom.Profiles;

/// <summary>
/// Entity qu?n lý thông tin ng??i thuê phòng
/// </summary>
public class Profile : AuditableEntity<Guid>
{
    /// <summary>
    /// H? và tên ??y ?? c?a ng??i thuê
    /// </summary>
    public string FullName { get; private set; }

    /// <summary>
    /// S? ?i?n tho?i liên l?c
    /// </summary>
    public string PhoneNumber { get; private set; }

    /// <summary>
    /// Email liên l?c
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// S? CMND/CCCD
    /// </summary>
    public string IdentityCard { get; private set; }

    /// <summary>
    /// Ngày sinh
    /// </summary>
    public DateTime? DateOfBirth { get; private set; }

    /// <summary>
    /// ??a ch? th??ng trú
    /// </summary>
    public string? PermanentAddress { get; private set; }

    /// <summary>
    /// Ngh? nghi?p
    /// </summary>
    public string? Occupation { get; private set; }

    /// <summary>
    /// ID phòng ?ang thuê
    /// </summary>
    public Guid? RoomId { get; private set; }

    /// <summary>
    /// Ngày b?t ??u thuê
    /// </summary>
    public DateTime? RentStartDate { get; private set; }

    /// <summary>
    /// Ngày k?t thúc thuê (n?u có)
    /// </summary>
    public DateTime? RentEndDate { get; private set; }

    /// <summary>
    /// S? ti?n ??t c?c
    /// </summary>
    public decimal? DepositAmount { get; private set; }

    /// <summary>
    /// Tr?ng thái ?ang thuê (true) hay ?ã tr? phòng (false)
    /// </summary>
    public bool IsActive { get; private set; }

    // Navigation properties - must be virtual for lazy loading
    /// <summary>
    /// Phòng ?ang thuê (lazy loading)
    /// </summary>
    public virtual Rooms.Room? Room { get; private set; }

    // Protected parameterless constructor for EF Core proxies
    protected Profile()
    {
        FullName = string.Empty;
        PhoneNumber = string.Empty;
        IdentityCard = string.Empty;
    }

    public Profile(
        Guid id,
        string fullName,
        string phoneNumber,
        string identityCard,
        string? email = null,
        DateTime? dateOfBirth = null,
        string? permanentAddress = null,
        string? occupation = null)
    {
        Id = id;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        IdentityCard = identityCard;
        Email = email;
        DateOfBirth = dateOfBirth;
        PermanentAddress = permanentAddress;
        Occupation = occupation;
        IsActive = true;
    }

    /// <summary>
    /// C?p nh?t thông tin cá nhân
    /// </summary>
    public void UpdatePersonalInfo(
        string fullName,
        string phoneNumber,
        string identityCard,
        string? email = null,
        DateTime? dateOfBirth = null,
        string? permanentAddress = null,
        string? occupation = null)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        IdentityCard = identityCard;
        Email = email;
        DateOfBirth = dateOfBirth;
        PermanentAddress = permanentAddress;
        Occupation = occupation;
    }

    /// <summary>
    /// Gán phòng cho ng??i thuê
    /// </summary>
    public void AssignRoom(Guid roomId, DateTime rentStartDate, decimal depositAmount)
    {
        RoomId = roomId;
        RentStartDate = rentStartDate;
        DepositAmount = depositAmount;
        IsActive = true;
        RentEndDate = null;
    }

    /// <summary>
    /// K?t thúc h?p ??ng thuê phòng
    /// </summary>
    public void EndRental(DateTime rentEndDate)
    {
        RentEndDate = rentEndDate;
        IsActive = false;
    }

    /// <summary>
    /// C?p nh?t s? ti?n ??t c?c
    /// </summary>
    public void UpdateDeposit(decimal depositAmount)
    {
        DepositAmount = depositAmount;
    }
}
