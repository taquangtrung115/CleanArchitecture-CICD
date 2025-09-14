using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.MotoGP.Season;

public static class Command
{
    public record CreateSeasonCommand(
        int Year,
        string Name,
        DateTime StartDate,
        DateTime EndDate,
        string? Description = null) : ICommand;

    public record UpdateSeasonCommand(
        Guid Id,
        string Name,
        DateTime StartDate,
        DateTime EndDate,
        string? Description) : ICommand;

    public record StartSeasonCommand(Guid Id) : ICommand;

    public record CompleteSeasonCommand(
        Guid Id,
        Guid? ChampionRiderId = null,
        Guid? ChampionTeamId = null) : ICommand;

    public record DeleteSeasonCommand(Guid Id) : ICommand;
}