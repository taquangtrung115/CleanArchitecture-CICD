using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.MotoGP.Rider;

public static class Command
{
    public record CreateRiderCommand(
        string FirstName,
        string LastName,
        int RacingNumber,
        string CountryCode,
        string CountryName,
        DateTime DateOfBirth,
        decimal Height,
        decimal Weight,
        string? Nickname = null) : ICommand;

    public record UpdateRiderPersonalInfoCommand(
        Guid Id,
        string FirstName,
        string LastName,
        string? Nickname,
        decimal Height,
        decimal Weight) : ICommand;

    public record UpdateRiderRacingNumberCommand(
        Guid Id,
        int RacingNumber) : ICommand;

    public record UpdateRiderPhotoCommand(
        Guid Id,
        string? Photo) : ICommand;

    public record TransferRiderToTeamCommand(
        Guid RiderId,
        Guid TeamId,
        Guid SeasonId,
        DateTime JoinDate) : ICommand;

    public record RemoveRiderFromTeamCommand(
        Guid RiderId,
        DateTime LeaveDate) : ICommand;

    public record RetireRiderCommand(
        Guid RiderId,
        DateTime RetirementDate) : ICommand;

    public record RiderComebackCommand(
        Guid RiderId) : ICommand;

    public record DeleteRiderCommand(Guid Id) : ICommand;
}