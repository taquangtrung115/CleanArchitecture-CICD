using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.MotoGP.Race;

public static class Command
{
    public record CreateRaceCommand(
        Guid SeasonId,
        string Name,
        string CircuitName,
        string CountryCode,
        string CountryName,
        DateTime RaceDate,
        DateTime FreeP1Date,
        DateTime FreeP2Date,
        DateTime QualifyingDate,
        DateTime WarmUpDate,
        int RoundNumber,
        decimal CircuitLength,
        int NumberOfLaps,
        string? Description = null) : ICommand;

    public record UpdateRaceDetailsCommand(
        Guid Id,
        string Name,
        string CircuitName,
        string CountryCode,
        string CountryName,
        DateTime RaceDate,
        DateTime FreeP1Date,
        DateTime FreeP2Date,
        DateTime QualifyingDate,
        DateTime WarmUpDate,
        decimal CircuitLength,
        int NumberOfLaps,
        string? Description) : ICommand;

    public record StartRaceCommand(Guid Id) : ICommand;

    public record CompleteRaceCommand(
        Guid Id,
        string? WeatherConditions = null) : ICommand;

    public record PostponeRaceCommand(
        Guid Id,
        DateTime NewRaceDate,
        DateTime NewFreeP1Date,
        DateTime NewFreeP2Date,
        DateTime NewQualifyingDate,
        DateTime NewWarmUpDate) : ICommand;

    public record CancelRaceCommand(
        Guid Id,
        string Reason) : ICommand;

    public record AddRaceEntryCommand(
        Guid RaceId,
        Guid RiderId,
        Guid TeamId,
        Guid BikeId,
        int StartingPosition) : ICommand;

    public record RemoveRaceEntryCommand(
        Guid RaceId,
        Guid RaceEntryId) : ICommand;

    public record UpdateRaceEntryResultCommand(
        Guid RaceId,
        Guid RaceEntryId,
        int? FinishPosition,
        TimeSpan? FinishTime,
        TimeSpan? BestLapTime,
        int? PointsEarned,
        bool IsFinisher,
        string? Notes) : ICommand;

    public record DeleteRaceCommand(Guid Id) : ICommand;
}