using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Rider;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using DemoCICD.Domain.Entities.MotoGP.ValueObjects;
using DemoCICD.Persistence;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.Rider;

public sealed class CreateRiderCommandHandler : ICommandHandler<Command.CreateRiderCommand>
{
    private readonly IRiderRepository _riderRepository;
    private readonly ApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public CreateRiderCommandHandler(
        IRiderRepository riderRepository,
        ApplicationDbContext context,
        IPublisher publisher)
    {
        _riderRepository = riderRepository;
        _context = context;
        _publisher = publisher;
    }

    public async Task<Result> Handle(Command.CreateRiderCommand request, CancellationToken cancellationToken)
    {
        // Check if racing number is available
        var isRacingNumberAvailable = await _riderRepository.IsRacingNumberAvailableAsync(request.RacingNumber, cancellationToken: cancellationToken);
        if (!isRacingNumberAvailable)
        {
            return Result.Failure(Error.Validation("RacingNumber.NotAvailable", $"Racing number {request.RacingNumber} is already taken"));
        }

        var country = new Country(request.CountryCode, request.CountryName, request.CountryFlag);
        
        var rider = Domain.Entities.MotoGP.TeamRiderManagement.Rider.Create(
            request.FirstName,
            request.LastName,
            request.RacingNumber,
            country,
            request.DateOfBirth,
            request.Height,
            request.Weight,
            request.Nickname);

        _riderRepository.Add(rider);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish domain event (if needed)
        //await _publisher.Publish(new DomainEvent.RiderCreated(rider.Id), cancellationToken);

        return Result.Success();
    }
}
