using AutoMapper;
using DemoCICD.Application.UserCases.V1.Queries.MotoGP.Rider;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Rider;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace DemoCICD.Architecture.Application.Queries.MotoGP.Rider;

public class GetRiderByIdQueryHandlerTests
{
    private readonly IRiderRepository _riderRepository;
    private readonly IMapper _mapper;
    private readonly GetRiderByIdQueryHandler _handler;

    public GetRiderByIdQueryHandlerTests()
    {
        _riderRepository = Substitute.For<IRiderRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetRiderByIdQueryHandler(_riderRepository, _mapper);
    }

    [Fact]
    public async Task Handle_WithValidId_ShouldReturnRiderResponse()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var query = new Query.GetRiderByIdQuery(riderId);

        var rider = CreateTestRider();
        var expectedResponse = new Response.RiderResponse(
            riderId,
            "Valentino",
            "Rossi",
            "Valentino Rossi",
            46,
            "IT",
            "Italy",
            "🇮🇹",
            new DateTime(1979, 2, 16),
            44,
            null,
            null,
            "The Doctor",
            null,
            181.0m,
            67.0m,
            false,
            new DateTime(1996, 8, 31),
            new DateTime(2021, 11, 14),
            25,
            false,
            DateTime.UtcNow,
            null);

        _riderRepository.FindByIdAsync(riderId, Arg.Any<CancellationToken>())
            .Returns(rider);

        _mapper.Map<Response.RiderResponse>(rider)
            .Returns(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(expectedResponse);

        await _riderRepository.Received(1).FindByIdAsync(riderId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<Response.RiderResponse>(rider);
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldReturnFailure()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var query = new Query.GetRiderByIdQuery(riderId);

        _riderRepository.FindByIdAsync(riderId, Arg.Any<CancellationToken>())
            .Returns((Domain.Entities.MotoGP.TeamRiderManagement.Rider?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rider.NotFound");
        result.Error.Message.Should().Contain($"Rider with ID {riderId} was not found");

        await _riderRepository.Received(1).FindByIdAsync(riderId, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<Response.RiderResponse>(Arg.Any<Domain.Entities.MotoGP.TeamRiderManagement.Rider>());
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var query = new Query.GetRiderByIdQuery(riderId);
        var cancellationToken = new CancellationToken();

        var rider = CreateTestRider();
        var expectedResponse = new Response.RiderResponse(
            riderId,
            "Test",
            "Rider",
            "Test Rider",
            1,
            "IT",
            "Italy",
            "🇮🇹",
            new DateTime(1990, 1, 1),
            33,
            null,
            null,
            null,
            null,
            180.0m,
            70.0m,
            true,
            null,
            null,
            0,
            false,
            DateTime.UtcNow,
            null);

        _riderRepository.FindByIdAsync(riderId, cancellationToken)
            .Returns(rider);

        _mapper.Map<Response.RiderResponse>(rider)
            .Returns(expectedResponse);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        await _riderRepository.Received(1).FindByIdAsync(riderId, cancellationToken);
    }

    [Fact]
    public async Task Handle_WithValidRider_ShouldCallMapperWithCorrectRider()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var query = new Query.GetRiderByIdQuery(riderId);

        var rider = CreateTestRider();
        var expectedResponse = new Response.RiderResponse(
            riderId,
            "Test",
            "Rider",
            "Test Rider",
            1,
            "IT",
            "Italy",
            "🇮🇹",
            new DateTime(1990, 1, 1),
            33,
            null,
            null,
            null,
            null,
            180.0m,
            70.0m,
            true,
            null,
            null,
            0,
            false,
            DateTime.UtcNow,
            null);

        _riderRepository.FindByIdAsync(riderId, Arg.Any<CancellationToken>())
            .Returns(rider);

        _mapper.Map<Response.RiderResponse>(rider)
            .Returns(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mapper.Received(1).Map<Response.RiderResponse>(Arg.Is<Domain.Entities.MotoGP.TeamRiderManagement.Rider>(r => r == rider));
    }

    private static Domain.Entities.MotoGP.TeamRiderManagement.Rider CreateTestRider()
    {
        var country = new Country("IT", "Italy", "🇮🇹");
        return Domain.Entities.MotoGP.TeamRiderManagement.Rider.Create(
            "Test",
            "Rider",
            1,
            country,
            new DateTime(1990, 1, 1),
            180m,
            70m,
            null);
    }
}