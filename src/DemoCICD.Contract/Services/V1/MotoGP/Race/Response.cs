namespace DemoCICD.Contract.Services.V1.MotoGP.Race;

public static class Response
{
    public record RaceResponse(
        Guid Id,
        Guid SeasonId,
        int SeasonYear,
        string Name,
        string CircuitName,
        string CountryCode,
        string CountryName,
        DateTime RaceDate,
        DateTime FreeP1Date,
        DateTime FreeP2Date,
        DateTime QualifyingDate,
        DateTime WarmUpDate,
        string Status,
        int RoundNumber,
        string? Description,
        string? WeatherConditions,
        decimal CircuitLength,
        int NumberOfLaps,
        int TotalEntries,
        DateTime CreatedDate,
        DateTime? ModifiedDate);

    public record RaceWithEntriesResponse(
        Guid Id,
        Guid SeasonId,
        int SeasonYear,
        string Name,
        string CircuitName,
        string CountryCode,
        string CountryName,
        DateTime RaceDate,
        DateTime FreeP1Date,
        DateTime FreeP2Date,
        DateTime QualifyingDate,
        DateTime WarmUpDate,
        string Status,
        int RoundNumber,
        string? Description,
        string? WeatherConditions,
        decimal CircuitLength,
        int NumberOfLaps,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        IEnumerable<RaceEntryResponse> Entries);

    public record RaceWithResultsResponse(
        Guid Id,
        Guid SeasonId,
        int SeasonYear,
        string Name,
        string CircuitName,
        string CountryCode,
        string CountryName,
        DateTime RaceDate,
        string Status,
        int RoundNumber,
        string? Description,
        string? WeatherConditions,
        decimal CircuitLength,
        int NumberOfLaps,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        IEnumerable<RaceResultResponse> Results);

    public record RaceEntryResponse(
        Guid Id,
        Guid RaceId,
        Guid RiderId,
        string RiderName,
        int RiderNumber,
        Guid TeamId,
        string TeamName,
        Guid BikeId,
        string BikeModel,
        int StartingPosition);

    public record RaceResultResponse(
        Guid Id,
        Guid RaceId,
        Guid RiderId,
        string RiderName,
        int RiderNumber,
        Guid TeamId,
        string TeamName,
        Guid BikeId,
        string BikeModel,
        int StartingPosition,
        int? FinishPosition,
        TimeSpan? FinishTime,
        TimeSpan? BestLapTime,
        int? PointsEarned,
        bool IsFinisher,
        string? Notes);

    public record RaceSummaryResponse(
        Guid Id,
        string Name,
        string CircuitName,
        string CountryCode,
        DateTime RaceDate,
        string Status,
        int RoundNumber);
}