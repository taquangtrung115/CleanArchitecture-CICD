namespace DemoCICD.Persistence.Constants;
internal static class TableNames
{
    // *********** Plural Nouns ***********
    internal const string Actions = nameof(Actions);
    internal const string Functions = nameof(Functions);
    internal const string ActionInFunctions = nameof(ActionInFunctions);
    internal const string Permissions = nameof(Permissions);

    internal const string AppUsers = nameof(AppUsers);
    internal const string AppRoles = nameof(AppRoles);
    internal const string AppUserRoles = nameof(AppUserRoles);
    internal const string Positions = nameof(Positions);

    internal const string AppUserClaims = nameof(AppUserClaims); // IdentityUserClaim
    internal const string AppRoleClaims = nameof(AppRoleClaims); // IdentityRoleClaim
    internal const string AppUserLogins = nameof(AppUserLogins); // IdentityLoginClaim
    internal const string AppUserTokens = nameof(AppUserTokens); // IdentityUserToken

    // *********** Singular Nouns ***********
    internal const string Product = nameof(Product);

    // *********** MotoGP Tables ***********
    internal const string Seasons = nameof(Seasons);
    internal const string Races = nameof(Races);
    internal const string RaceEntries = nameof(RaceEntries);
    internal const string Teams = nameof(Teams);
    internal const string Riders = nameof(Riders);
    internal const string RiderTeamHistories = nameof(RiderTeamHistories);
    internal const string Bikes = nameof(Bikes);
    internal const string News = nameof(News);
    internal const string Videos = nameof(Videos);

    // *********** RentalRoom Tables ***********
    internal const string Rooms = nameof(Rooms);
    internal const string Profiles = nameof(Profiles);
    internal const string Locations = nameof(Locations);
    internal const string Bills = nameof(Bills);
    internal const string BillDetails = nameof(BillDetails);
}
