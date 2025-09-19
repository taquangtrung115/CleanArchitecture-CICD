using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Rider;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Persistence;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.Rider;

public sealed class UpdateRiderPersonalInfoCommandHandler : ICommandHandler<Command.UpdateRiderPersonalInfoCommand>
{
    private readonly IRiderRepository _riderRepository;
    private readonly ApplicationDbContext _context;

    public UpdateRiderPersonalInfoCommandHandler(
        IRiderRepository riderRepository,
        ApplicationDbContext context)
    {
        _riderRepository = riderRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.UpdateRiderPersonalInfoCommand request, CancellationToken cancellationToken)
    {
        var rider = await _riderRepository.FindByIdAsync(request.Id, cancellationToken);
        if (rider == null)
        {
            return Result.Failure(Error.NotFound("Rider.NotFound", $"Rider with ID {request.Id} was not found"));
        }

        rider.UpdatePersonalInfo(
            request.FirstName,
            request.LastName,
            request.Nickname,
            request.Height,
            request.Weight);

        _riderRepository.Update(rider);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}