namespace DemoCICD.Contract.Services.V1.MotoGP.Rider;

public static class Response
{
    public record RiderResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string FullName,
        int RacingNumber,
        string CountryCode,
        string CountryName,
        string CountryFlag,
        DateTime DateOfBirth,
        int Age,
        Guid? CurrentTeamId,
        string? CurrentTeamName,
        string? Nickname,
        string? Photo,
        decimal Height,
        decimal Weight,
        bool IsActive,
        DateTime? DebutDate,
        DateTime? RetirementDate,
        int YearsInMotoGP,
        bool IsCurrentlyInTeam,
        DateTime CreatedDate,
        DateTime? ModifiedDate);

    public record RiderWithHistoryResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string FullName,
        int RacingNumber,
        string CountryCode,
        string CountryName,
        DateTime DateOfBirth,
        int Age,
        Guid? CurrentTeamId,
        string? CurrentTeamName,
        string? Nickname,
        string? Photo,
        decimal Height,
        decimal Weight,
        bool IsActive,
        DateTime? DebutDate,
        DateTime? RetirementDate,
        int YearsInMotoGP,
        bool IsCurrentlyInTeam,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        IEnumerable<RiderTeamHistoryResponse> TeamHistory);

    public record RiderTeamHistoryResponse(
        Guid Id,
        Guid RiderId,
        Guid TeamId,
        string TeamName,
        Guid SeasonId,
        int SeasonYear,
        DateTime StartDate,
        DateTime? EndDate,
        bool IsCurrentTeam);

    public record RiderSummaryResponse(
        Guid Id,
        string FullName,
        int RacingNumber,
        string CountryCode,
        string? CurrentTeamName,
        bool IsActive);
}