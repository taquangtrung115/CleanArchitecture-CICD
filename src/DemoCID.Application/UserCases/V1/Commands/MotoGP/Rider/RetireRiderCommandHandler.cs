using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Rider;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Persistence;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.Rider;

public sealed class RetireRiderCommandHandler : ICommandHandler<Command.RetireRiderCommand>
{
    private readonly IRiderRepository _riderRepository;
    private readonly ApplicationDbContext _context;

    public RetireRiderCommandHandler(
        IRiderRepository riderRepository,
        ApplicationDbContext context)
    {
        _riderRepository = riderRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.RetireRiderCommand request, CancellationToken cancellationToken)
    {
        var rider = await _riderRepository.FindByIdAsync(request.RiderId, cancellationToken);
        if (rider == null)
        {
            return Result.Failure(Error.NotFound("Rider.NotFound", $"Rider with ID {request.RiderId} was not found"));
        }

        try
        {
            rider.Retire(request.RetirementDate);
            _riderRepository.Update(rider);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(Error.Validation("Retirement.Failed", ex.Message));
        }
    }
}