using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Team;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.ValueObjects;
using DemoCICD.Persistence;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.Team;

public sealed class CreateTeamCommandHandler : ICommandHandler<Command.CreateTeamCommand>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public CreateTeamCommandHandler(
        ITeamRepository teamRepository,
        ApplicationDbContext context,
        IPublisher publisher)
    {
        _teamRepository = teamRepository;
        _context = context;
        _publisher = publisher;
    }

    public async Task<Result> Handle(Command.CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var country = new Country(request.CountryCode, request.CountryName, request.CountryFlag);
        
        var team = Domain.Entities.MotoGP.TeamRiderManagement.Team.Create(
            request.Name,
            request.ShortName,
            country,
            request.FoundedYear,
            request.Description);

        _teamRepository.Add(team);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish domain event (if needed)
        // await _publisher.Publish(new DomainEvent.TeamCreated(team.Id), cancellationToken);

        return Result.Success();
    }
}