using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Rider;
using DemoCICD.Presentation.APIs.MotoGP;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace DemoCICD.UnitTests.Presentation.APIs.MotoGP;

public class RiderApiTests
{
    private readonly ISender _sender;

    public RiderApiTests()
    {
        _sender = Substitute.For<ISender>();
    }

    [Fact]
    public async Task CreateRider_WithValidCommand_ReturnsCreatedResult()
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

        var expectedResponse = Result.Success();
        _sender.Send(command).Returns(expectedResponse);

        // Act
        var result = await RiderApi.CreateRider(_sender, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task CreateRider_WithFailedCommand_ReturnsFailureResult()
    {
        // Arrange
        var command = new Command.CreateRiderCommand(
            "",
            "",
            46,
            "IT",
            "Italy",
            "🇮🇹",
            new DateTime(1979, 2, 16),
            181.0m,
            67.0m,
            "The Doctor");

        var errorResponse = Result.Failure(new Error("RIDER.INVALID_DATA", "Invalid rider data"));
        _sender.Send(command).Returns(errorResponse);

        // Act
        var result = await RiderApi.CreateRider(_sender, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task GetRiders_WithDefaultParameters_ReturnsOkResult()
    {
        // Arrange
        var expectedResponse = PagedResult<Response.RiderResponse>.Create(
            [],
            1,
            10,
            0);

        _sender.Send(Arg.Any<Query.GetRidersQuery>()).Returns(expectedResponse);

        // Act
        var result = await RiderApi.GetRiders(_sender);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetRidersQuery>());
    }

    [Fact]
    public async Task GetRiders_WithFilterParameters_ReturnsOkResult()
    {
        // Arrange
        var expectedResponse = PagedResult<Response.RiderResponse>.Create(
            [],
            1,
            10,
            0);

        _sender.Send(Arg.Any<Query.GetRidersQuery>()).Returns(expectedResponse);

        // Act
        var result = await RiderApi.GetRiders(
            _sender,
            searchTerm: "Rossi",
            sortColumn: "lastName",
            sortOrder: "asc",
            pageIndex: 1,
            pageSize: 10,
            isActive: true,
            countryCode: "IT",
            teamId: Guid.NewGuid());

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetRidersQuery>());
    }

    [Fact]
    public async Task GetRiderById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var expectedResponse = Result.Success(new Response.RiderResponse(
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
            null));

        _sender.Send(Arg.Any<Query.GetRiderByIdQuery>()).Returns(expectedResponse);

        // Act
        var result = await RiderApi.GetRiderById(_sender, riderId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetRiderByIdQuery>());
    }

    [Fact]
    public async Task GetRiderById_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var errorResponse = Result.Failure<Response.RiderResponse>(new Error("RIDER.NOT_FOUND", "Rider not found"));

        _sender.Send(Arg.Any<Query.GetRiderByIdQuery>()).Returns(errorResponse);

        // Act
        var result = await RiderApi.GetRiderById(_sender, riderId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetRiderByIdQuery>());
    }

    [Fact]
    public async Task GetRiderByRacingNumber_WithValidNumber_ReturnsOkResult()
    {
        // Arrange
        var racingNumber = 46;
        var expectedResponse = Result.Success(new Response.RiderResponse(
            Guid.NewGuid(),
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
            null));

        _sender.Send(Arg.Any<Query.GetRiderByRacingNumberQuery>()).Returns(expectedResponse);

        // Act
        var result = await RiderApi.GetRiderByRacingNumber(_sender, racingNumber);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetRiderByRacingNumberQuery>());
    }

    [Fact]
    public async Task UpdateRiderPersonalInfo_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var request = new RiderApi.UpdateRiderPersonalInfoRequest(
            "Marc",
            "Marquez",
            "MM93",
            168.0m,
            59.0m);

        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.UpdateRiderPersonalInfoCommand>()).Returns(expectedResponse);

        // Act
        var result = await RiderApi.UpdateRiderPersonalInfo(_sender, riderId, request);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.UpdateRiderPersonalInfoCommand>());
    }

    [Fact]
    public async Task TransferRiderToTeam_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var teamId = Guid.NewGuid();
        var seasonId = Guid.NewGuid();
        var request = new RiderApi.TransferRiderRequest(teamId, seasonId, DateTime.UtcNow);

        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.TransferRiderToTeamCommand>()).Returns(expectedResponse);

        // Act
        var result = await RiderApi.TransferRiderToTeam(_sender, riderId, request);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.TransferRiderToTeamCommand>());
    }

    [Fact]
    public async Task RetireRider_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var request = new RiderApi.RetireRiderRequest(DateTime.UtcNow);

        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.RetireRiderCommand>()).Returns(expectedResponse);

        // Act
        var result = await RiderApi.RetireRider(_sender, riderId, request);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.RetireRiderCommand>());
    }

    [Fact]
    public async Task RiderComeback_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.RiderComebackCommand>()).Returns(expectedResponse);

        // Act
        var result = await RiderApi.RiderComeback(_sender, riderId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.RiderComebackCommand>());
    }

    [Fact]
    public async Task DeleteRider_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var expectedResponse = Result.Success();
        _sender.Send(Arg.Any<Command.DeleteRiderCommand>()).Returns(expectedResponse);

        // Act
        var result = await RiderApi.DeleteRider(_sender, riderId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.DeleteRiderCommand>());
    }

    [Fact]
    public async Task DeleteRider_WithFailure_ReturnsFailureResult()
    {
        // Arrange
        var riderId = Guid.NewGuid();
        var errorResponse = Result.Failure(new Error("RIDER.NOT_FOUND", "Rider not found"));
        _sender.Send(Arg.Any<Command.DeleteRiderCommand>()).Returns(errorResponse);

        // Act
        var result = await RiderApi.DeleteRider(_sender, riderId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Command.DeleteRiderCommand>());
    }
}