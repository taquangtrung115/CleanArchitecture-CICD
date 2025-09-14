namespace DemoCICD.Contract.Services.V1.MotoGP.Season;

public static class Response
{
    public record SeasonResponse(
        Guid Id,
        int Year,
        string Name,
        DateTime StartDate,
        DateTime EndDate,
        string? Description,
        bool IsActive,
        bool IsCompleted,
        Guid? ChampionRiderId,
        string? ChampionRiderName,
        Guid? ChampionTeamId,
        string? ChampionTeamName,
        int TotalRaces,
        int CompletedRaces,
        DateTime CreatedDate,
        DateTime? ModifiedDate);

    public record SeasonWithRacesResponse(
        Guid Id,
        int Year,
        string Name,
        DateTime StartDate,
        DateTime EndDate,
        string? Description,
        bool IsActive,
        bool IsCompleted,
        Guid? ChampionRiderId,
        string? ChampionRiderName,
        Guid? ChampionTeamId,
        string? ChampionTeamName,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        IEnumerable<SeasonRaceResponse> Races);

    public record SeasonWithStandingsResponse(
        Guid Id,
        int Year,
        string Name,
        DateTime StartDate,
        DateTime EndDate,
        string? Description,
        bool IsActive,
        bool IsCompleted,
        Guid? ChampionRiderId,
        string? ChampionRiderName,
        Guid? ChampionTeamId,
        string? ChampionTeamName,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        IEnumerable<RiderStandingResponse> RiderStandings,
        IEnumerable<TeamStandingResponse> TeamStandings);

    public record SeasonWithFullDetailsResponse(
        Guid Id,
        int Year,
        string Name,
        DateTime StartDate,
        DateTime EndDate,
        string? Description,
        bool IsActive,
        bool IsCompleted,
        Guid? ChampionRiderId,
        string? ChampionRiderName,
        Guid? ChampionTeamId,
        string? ChampionTeamName,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        IEnumerable<SeasonRaceResponse> Races,
        IEnumerable<RiderStandingResponse> RiderStandings,
        IEnumerable<TeamStandingResponse> TeamStandings);

    public record SeasonRaceResponse(
        Guid Id,
        string Name,
        string CircuitName,
        string CountryCode,
        DateTime RaceDate,
        string Status,
        int RoundNumber);

    public record RiderStandingResponse(
        Guid RiderId,
        string RiderName,
        int RiderNumber,
        Guid? TeamId,
        string? TeamName,
        int Position,
        int Points,
        int Wins,
        int Podiums,
        int PointFinishes);

    public record TeamStandingResponse(
        Guid TeamId,
        string TeamName,
        int Position,
        int Points,
        int Wins,
        int Podiums,
        int PointFinishes);

    public record SeasonSummaryResponse(
        Guid Id,
        int Year,
        string Name,
        bool IsActive,
        bool IsCompleted,
        string? ChampionRiderName);
}