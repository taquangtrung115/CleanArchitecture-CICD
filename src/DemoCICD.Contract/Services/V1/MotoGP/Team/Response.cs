namespace DemoCICD.Contract.Services.V1.MotoGP.Team;

public static class Response
{
    public record TeamResponse(
        Guid Id,
        string Name,
        string ShortName,
        string CountryCode,
        string CountryName,
        string? Logo,
        string? Website,
        DateTime FoundedYear,
        string? Description,
        string? PrimaryColor,
        string? SecondaryColor,
        bool IsActive,
        int ActiveRidersCount,
        int BikesCount,
        DateTime CreatedDate,
        DateTime? ModifiedDate);

    public record TeamWithRidersResponse(
        Guid Id,
        string Name,
        string ShortName,
        string CountryCode,
        string CountryName,
        string? Logo,
        string? Website,
        DateTime FoundedYear,
        string? Description,
        string? PrimaryColor,
        string? SecondaryColor,
        bool IsActive,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        IEnumerable<TeamRiderResponse> Riders);

    public record TeamWithBikesResponse(
        Guid Id,
        string Name,
        string ShortName,
        string CountryCode,
        string CountryName,
        string? Logo,
        string? Website,
        DateTime FoundedYear,
        string? Description,
        string? PrimaryColor,
        string? SecondaryColor,
        bool IsActive,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        IEnumerable<TeamBikeResponse> Bikes);

    public record TeamWithFullDetailsResponse(
        Guid Id,
        string Name,
        string ShortName,
        string CountryCode,
        string CountryName,
        string? Logo,
        string? Website,
        DateTime FoundedYear,
        string? Description,
        string? PrimaryColor,
        string? SecondaryColor,
        bool IsActive,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        IEnumerable<TeamRiderResponse> Riders,
        IEnumerable<TeamBikeResponse> Bikes);

    public record TeamRiderResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string FullName,
        int RacingNumber,
        string CountryCode,
        bool IsActive);

    public record TeamBikeResponse(
        Guid Id,
        string Manufacturer,
        string Model,
        int Year,
        string FullName,
        string? ChassisNumber,
        string? EngineNumber,
        bool IsActive);

    public record TeamSummaryResponse(
        Guid Id,
        string Name,
        string ShortName,
        string CountryCode,
        bool IsActive,
        int ActiveRidersCount);
}