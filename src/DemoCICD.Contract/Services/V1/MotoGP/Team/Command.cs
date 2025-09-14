using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.MotoGP.Team;

public static class Command
{
    public record CreateTeamCommand(
        string Name,
        string ShortName,
        string CountryCode,
        string CountryName,
        string CountryFlag,
        DateTime FoundedYear,
        string? Description = null) : ICommand;

    public record UpdateTeamDetailsCommand(
        Guid Id,
        string Name,
        string ShortName,
        string CountryCode,
        string CountryName,
        string CountryFlag,
        string? Description) : ICommand;

    public record UpdateTeamVisualIdentityCommand(
        Guid Id,
        string? Logo,
        string? Website,
        string? PrimaryColor,
        string? SecondaryColor) : ICommand;

    public record ActivateTeamCommand(Guid Id) : ICommand;

    public record DeactivateTeamCommand(Guid Id) : ICommand;

    public record AddRiderToTeamCommand(
        Guid TeamId,
        Guid RiderId,
        Guid SeasonId,
        DateTime JoinDate) : ICommand;

    public record RemoveRiderFromTeamCommand(
        Guid TeamId,
        Guid RiderId,
        DateTime LeaveDate) : ICommand;

    public record AssignBikeToTeamCommand(
        Guid TeamId,
        Guid BikeId) : ICommand;

    public record RemoveBikeFromTeamCommand(
        Guid TeamId,
        Guid BikeId) : ICommand;

    public record DeleteTeamCommand(Guid Id) : ICommand;
}