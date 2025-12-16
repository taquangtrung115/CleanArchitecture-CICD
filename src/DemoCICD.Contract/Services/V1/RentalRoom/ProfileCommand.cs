using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Commands cho qu?n lý ng??i thuê phòng
/// </summary>
public static class ProfileCommand
{
    /// <summary>
    /// Command t?o profile ng??i thuê m?i
    /// </summary>
    public record CreateProfileCommand(
        string FullName,
        string PhoneNumber,
        string IdentityCard,
        string? Email = null,
        DateTime? DateOfBirth = null,
        string? PermanentAddress = null,
        string? Occupation = null) : ICommand<Guid>;

    /// <summary>
    /// Command c?p nh?t thông tin ng??i thuê
    /// </summary>
    public record UpdateProfileCommand(
        Guid Id,
        string FullName,
        string PhoneNumber,
        string IdentityCard,
        string? Email = null,
        DateTime? DateOfBirth = null,
        string? PermanentAddress = null,
        string? Occupation = null) : ICommand;

    /// <summary>
    /// Command xóa profile ng??i thuê
    /// </summary>
    public record DeleteProfileCommand(Guid Id) : ICommand;

    /// <summary>
    /// Command gán phòng cho ng??i thuê
    /// </summary>
    public record AssignRoomToProfileCommand(
        Guid ProfileId,
        Guid RoomId,
        DateTime RentStartDate,
        decimal DepositAmount) : ICommand;

    /// <summary>
    /// Command k?t thúc h?p ??ng thuê
    /// </summary>
    public record EndRentalCommand(
        Guid ProfileId,
        DateTime RentEndDate) : ICommand;
}
