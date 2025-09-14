using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Rider;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Persistence;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.Rider;

public sealed class TransferRiderToTeamCommandHandler : ICommandHandler<Command.TransferRiderToTeamCommand>
{
    private readonly IRiderRepository _riderRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ApplicationDbContext _context;

    public TransferRiderToTeamCommandHandler(
        IRiderRepository riderRepository,
        ITeamRepository teamRepository,
        ApplicationDbContext context)
    {
        _riderRepository = riderRepository;
        _teamRepository = teamRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.TransferRiderToTeamCommand request, CancellationToken cancellationToken)
    {
        var rider = await _riderRepository.FindByIdAsync(request.RiderId, cancellationToken);
        if (rider == null)
        {
            return Result.Failure(Error.NotFound("Rider.NotFound", $"Rider with ID {request.RiderId} was not found"));
        }

        var team = await _teamRepository.FindByIdAsync(request.TeamId, cancellationToken);
        if (team == null)
        {
            return Result.Failure(Error.NotFound("Team.NotFound", $"Team with ID {request.TeamId} was not found"));
        }

        if (!team.IsActive)
        {
            return Result.Failure(Error.Validation("Team.NotActive", "Cannot transfer rider to inactive team"));
        }

        try
        {
            rider.JoinTeam(request.TeamId, request.JoinDate, request.SeasonId);
            _riderRepository.Update(rider);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(Error.Validation("Transfer.Failed", ex.Message));
        }
    }
}