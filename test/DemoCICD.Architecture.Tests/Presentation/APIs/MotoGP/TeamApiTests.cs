using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Team;
using DemoCICD.Presentation.APIs.MotoGP;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace DemoCICD.Architecture.Presentation.APIs.MotoGP;

public class TeamApiTests
{
    private readonly ISender _sender;

    public TeamApiTests()
    {
        _sender = Substitute.For<ISender>();
    }

    [Fact]
    public async Task CreateTeam_WithValidCommand_ReturnsCreatedResult()
    {
        // Arrange
        var command = new Command.CreateTeamCommand(
            "Yamaha Factory Racing",
            "Yamaha",
            "JP",
            "Japan",
            "🇯🇵",
            new DateTime(1955, 1, 1),
            "Official Yamaha MotoGP factory team");

        var expectedResponse = Result.Success();
        _sender.Send(command).Returns(expectedResponse);

        // Act
        var result = await TeamApi.CreateTeam(_sender, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task CreateTeam_WithFailedCommand_ReturnsFailureResult()
    {
        // Arrange
        var command = new Command.CreateTeamCommand(
            "",
            "",
            "JP",
            "Japan",
            "🇯🇵",
            new DateTime(1955, 1, 1),
            "Official Yamaha MotoGP factory team");

        var errorResponse = Result.Failure(new Error("TEAM.INVALID_DATA", "Invalid team data"));
        _sender.Send(command).Returns(errorResponse);

        // Act
        var result = await TeamApi.CreateTeam(_sender, command);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(command);
    }

    [Fact]
    public async Task GetTeams_WithDefaultParameters_ReturnsOkResult()
    {
        // Arrange
        var expectedResponse = PagedResult<Response.TeamResponse>.Create(
            [],
            1,
            10,
            0);

        _sender.Send(Arg.Any<Query.GetTeamsQuery>()).Returns(expectedResponse);

        // Act
        var result = await TeamApi.GetTeams(_sender);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetTeamsQuery>());
    }

    [Fact]
    public async Task GetTeams_WithFilterParameters_ReturnsOkResult()
    {
        // Arrange
        var expectedResponse = PagedResult<Response.TeamResponse>.Create(
            [],
            1,
            10,
            0);

        _sender.Send(Arg.Any<Query.GetTeamsQuery>()).Returns(expectedResponse);

        // Act
        var result = await TeamApi.GetTeams(
            _sender,
            searchTerm: "Yamaha",
            sortColumn: "name",
            sortOrder: "asc",
            pageIndex: 1,
            pageSize: 10,
            isActive: true,
            countryCode: "JP");

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetTeamsQuery>());
    }

    [Fact]
    public async Task GetTeamById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var expectedResponse = Result.Success(new Response.TeamResponse(
            teamId,
            "Yamaha Factory Racing",
            "Yamaha",
            "JP",
            "Japan",
            null,
            null,
            new DateTime(1955, 1, 1),
            "Official Yamaha MotoGP factory team",
            null,
            null,
            true,
            2,
            4,
            DateTime.UtcNow,
            null));

        _sender.Send(Arg.Any<Query.GetTeamByIdQuery>()).Returns(expectedResponse);

        // Act
        var result = await TeamApi.GetTeamById(_sender, teamId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetTeamByIdQuery>());
    }

    [Fact]
    public async Task GetTeamById_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var errorResponse = Result.Failure<Response.TeamResponse>(new Error("TEAM.NOT_FOUND", "Team not found"));

        _sender.Send(Arg.Any<Query.GetTeamByIdQuery>()).Returns(errorResponse);

        // Act
        var result = await TeamApi.GetTeamById(_sender, teamId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetTeamByIdQuery>());
    }

    [Fact]
    public async Task GetTeamWithRiders_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var expectedResponse = Result.Success(new Response.TeamWithRidersResponse(
            teamId,
            "Yamaha Factory Racing",
            "Yamaha",
            "JP",
            "Japan",
            null,
            null,
            new DateTime(1955, 1, 1),
            "Official Yamaha MotoGP factory team",
            null,
            null,
            true,
            DateTime.UtcNow,
            null,
            []));

        _sender.Send(Arg.Any<Query.GetTeamWithRidersQuery>()).Returns(expectedResponse);

        // Act
        var result = await TeamApi.GetTeamWithRiders(_sender, teamId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetTeamWithRidersQuery>());
    }

    [Fact]
    public async Task GetTeamWithRiders_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var errorResponse = Result.Failure<Response.TeamWithRidersResponse>(new Error("TEAM.NOT_FOUND", "Team not found"));

        _sender.Send(Arg.Any<Query.GetTeamWithRidersQuery>()).Returns(errorResponse);

        // Act
        var result = await TeamApi.GetTeamWithRiders(_sender, teamId);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(Arg.Any<Query.GetTeamWithRidersQuery>());
    }

    [Fact]
    public async Task CreateTeam_WithNullCommand_HandlesGracefully()
    {
        // Arrange
        Command.CreateTeamCommand nullCommand = null!;
        var errorResponse = Result.Failure(new Error("TEAM.INVALID_REQUEST", "Invalid request"));

        _sender.Send(Arg.Any<Command.CreateTeamCommand>()).Returns(errorResponse);

        // Act
        var result = await TeamApi.CreateTeam(_sender, nullCommand);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(nullCommand);
    }
}
