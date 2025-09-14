using DemoCICD.Application.UserCases.V1.Commands.MotoGP.Rider;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Rider;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Persistence;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace DemoCICD.UnitTests.Application.Commands.MotoGP.Rider;

public class CreateRiderCommandHandlerTests
{
    private readonly IRiderRepository _riderRepository;
    private readonly ApplicationDbContext _context;
    private readonly IPublisher _publisher;
    private readonly CreateRiderCommandHandler _handler;

    public CreateRiderCommandHandlerTests()
    {
        _riderRepository = Substitute.For<IRiderRepository>();
        _context = Substitute.For<ApplicationDbContext>();
        _publisher = Substitute.For<IPublisher>();
        _handler = new CreateRiderCommandHandler(_riderRepository, _context, _publisher);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnSuccess()
    {
        // Arrange
        var command = new Command.CreateRiderCommand(
            "Valentino",
            "Rossi",
            46,
            "IT",
            "Italy",
            "🇮🇹",
            new DateTime(1979, 2, 16),
            181.0m,
            67.0m,
            "The Doctor");

        _riderRepository.IsRacingNumberAvailableAsync(46, cancellationToken: Arg.Any<CancellationToken>())
            .Returns(true);

        _context.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        
        _riderRepository.Received(1).Add(Arg.Any<Domain.Entities.MotoGP.TeamRiderManagement.Rider>());
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithTakenRacingNumber_ShouldReturnFailure()
    {
        // Arrange
        var command = new Command.CreateRiderCommand(
            "Marc",
            "Marquez",
            46, // Already taken racing number
            "ES",
            "Spain",
            "🇪🇸",
            new DateTime(1993, 2, 17),
            168.0m,
            59.0m,
            "MM93");

        _riderRepository.IsRacingNumberAvailableAsync(46, cancellationToken: Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("RacingNumber.NotAvailable");
        result.Error.Message.Should().Contain("Racing number 46 is already taken");
        
        _riderRepository.DidNotReceive().Add(Arg.Any<Domain.Entities.MotoGP.TeamRiderManagement.Rider>());
        await _context.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCallRepositoryMethods()
    {
        // Arrange
        var command = new Command.CreateRiderCommand(
            "Jorge",
            "Lorenzo",
            99,
            "ES",
            "Spain",
            "🇪🇸",
            new DateTime(1987, 5, 4),
            168.0m,
            64.0m,
            "JL99");

        _riderRepository.IsRacingNumberAvailableAsync(99, cancellationToken: Arg.Any<CancellationToken>())
            .Returns(true);

        _context.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        await _riderRepository.Received(1).IsRacingNumberAvailableAsync(99, cancellationToken: Arg.Any<CancellationToken>());
        _riderRepository.Received(1).Add(Arg.Is<Domain.Entities.MotoGP.TeamRiderManagement.Rider>(r => 
            r.FirstName == "Jorge" && 
            r.LastName == "Lorenzo" && 
            r.RacingNumber == 99));
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var command = new Command.CreateRiderCommand(
            "Casey",
            "Stoner",
            27,
            "AU",
            "Australia",
            "🇦🇺",
            new DateTime(1985, 10, 16),
            175.0m,
            65.0m,
            "CS27");

        var cancellationToken = new CancellationToken();
        
        _riderRepository.IsRacingNumberAvailableAsync(27, cancellationToken: cancellationToken)
            .Returns(true);

        _context.SaveChangesAsync(cancellationToken)
            .Returns(1);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        await _riderRepository.Received(1).IsRacingNumberAvailableAsync(27, cancellationToken: cancellationToken);
        await _context.Received(1).SaveChangesAsync(cancellationToken);
    }
}