using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Domain.Events.MotoGP;

public record SeasonStarted(Guid Id, Guid SeasonId, int Year, string Name, DateTime StartDate) : IDomainEvent
{
    public SeasonStarted(Guid seasonId, int year, string name, DateTime startDate) 
        : this(Guid.NewGuid(), seasonId, year, name, startDate) { }
}

public record SeasonCompleted(Guid Id, Guid SeasonId, int Year, string Name, DateTime EndDate, Guid? ChampionRiderId, Guid? ChampionTeamId) : IDomainEvent
{
    public SeasonCompleted(Guid seasonId, int year, string name, DateTime endDate, Guid? championRiderId, Guid? championTeamId) 
        : this(Guid.NewGuid(), seasonId, year, name, endDate, championRiderId, championTeamId) { }
}

public record RaceFinished(Guid Id, Guid RaceId, Guid SeasonId, string RaceName, string CircuitName, DateTime RaceDate, Guid? WinnerRiderId, Guid? WinnerTeamId) : IDomainEvent
{
    public RaceFinished(Guid raceId, Guid seasonId, string raceName, string circuitName, DateTime raceDate, Guid? winnerRiderId, Guid? winnerTeamId) 
        : this(Guid.NewGuid(), raceId, seasonId, raceName, circuitName, raceDate, winnerRiderId, winnerTeamId) { }
}

public record RiderTransferred(Guid Id, Guid RiderId, string RiderName, Guid? FromTeamId, Guid ToTeamId, Guid SeasonId, DateTime TransferDate) : IDomainEvent
{
    public RiderTransferred(Guid riderId, string riderName, Guid? fromTeamId, Guid toTeamId, Guid seasonId, DateTime transferDate) 
        : this(Guid.NewGuid(), riderId, riderName, fromTeamId, toTeamId, seasonId, transferDate) { }
}

public record NewsPublished(Guid Id, Guid NewsId, string Title, string Category, Guid AuthorId, DateTime PublishedDate, bool IsBreaking) : IDomainEvent
{
    public NewsPublished(Guid newsId, string title, string category, Guid authorId, DateTime publishedDate, bool isBreaking) 
        : this(Guid.NewGuid(), newsId, title, category, authorId, publishedDate, isBreaking) { }
}

public record VideoPublished(Guid Id, Guid VideoId, string Title, string Type, DateTime PublishedDate) : IDomainEvent
{
    public VideoPublished(Guid videoId, string title, string type, DateTime publishedDate) 
        : this(Guid.NewGuid(), videoId, title, type, publishedDate) { }
}

public record RaceScheduleChanged(Guid Id, Guid RaceId, string RaceName, DateTime OldDate, DateTime NewDate, string Reason) : IDomainEvent
{
    public RaceScheduleChanged(Guid raceId, string raceName, DateTime oldDate, DateTime newDate, string reason) 
        : this(Guid.NewGuid(), raceId, raceName, oldDate, newDate, reason) { }
}

public record ChampionshipPointsUpdated(Guid Id, Guid SeasonId, Guid RiderId, int OldPoints, int NewPoints, string Reason) : IDomainEvent
{
    public ChampionshipPointsUpdated(Guid seasonId, Guid riderId, int oldPoints, int newPoints, string reason) 
        : this(Guid.NewGuid(), seasonId, riderId, oldPoints, newPoints, reason) { }
}