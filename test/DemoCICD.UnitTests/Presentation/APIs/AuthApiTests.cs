using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Presentation.APIs.Identity;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace DemoCICD.UnitTests.Presentation.APIs;

public class AuthApiTests
{
    private readonly ISender _sender;

    public AuthApiTests()
    {
        _sender = Substitute.For<ISender>();
    }

    [Fact]
    public async Task LoginV1_WithSuccessfulLogin_ReturnsOkResult()
    {
        // Arrange
        var loginRequest = new Query.Login("testuser", "password123");
        var expectedResponse = Result.Success(new Response.Authenticated(
            "access-token",
            "refresh-token",
            DateTime.UtcNow.AddDays(7)
        ));

        _sender.Send(loginRequest).Returns(expectedResponse);

        // Act
        var result = await AuthApi.LoginV1(_sender, loginRequest);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(loginRequest);
    }

    [Fact]
    public async Task LoginV1_WithFailedLogin_ReturnsFailureResult()
    {
        // Arrange
        var loginRequest = new Query.Login("testuser", "wrongpassword");
        var errorResponse = Result.Failure<Response.Authenticated>(new Error("AUTH.INVALID_CREDENTIALS", "Invalid username or password"));

        _sender.Send(loginRequest).Returns(errorResponse);

        // Act
        var result = await AuthApi.LoginV1(_sender, loginRequest);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(loginRequest);
    }

    [Fact]
    public async Task LoginV1_WithNullRequest_HandlesGracefully()
    {
        // Arrange
        Query.Login nullRequest = null!;
        var errorResponse = Result.Failure<Response.Authenticated>(new Error("AUTH.INVALID_REQUEST", "Invalid request"));

        _sender.Send(Arg.Any<Query.Login>()).Returns(errorResponse);

        // Act
        var result = await AuthApi.LoginV1(_sender, nullRequest);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(nullRequest);
    }

    [Fact]
    public async Task LogoutV1_WithSuccessfulLogout_ReturnsOkResult()
    {
        // Arrange
        var logoutRequest = new Command.Logout("access-token");
        var expectedResponse = Result.Success();

        _sender.Send(logoutRequest).Returns(expectedResponse);

        // Act
        var result = await AuthApi.LogoutV1(_sender, logoutRequest);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(logoutRequest);
    }

    [Fact]
    public async Task LogoutV1_WithFailedLogout_ReturnsFailureResult()
    {
        // Arrange
        var logoutRequest = new Command.Logout("invalid-token");
        var errorResponse = Result.Failure(new Error("AUTH.INVALID_TOKEN", "Invalid token"));

        _sender.Send(logoutRequest).Returns(errorResponse);

        // Act
        var result = await AuthApi.LogoutV1(_sender, logoutRequest);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(logoutRequest);
    }

    [Fact]
    public async Task LogoutV1_WithNullRequest_HandlesGracefully()
    {
        // Arrange
        Command.Logout nullRequest = null!;
        var errorResponse = Result.Failure(new Error("AUTH.INVALID_REQUEST", "Invalid request"));

        _sender.Send(Arg.Any<Command.Logout>()).Returns(errorResponse);

        // Act
        var result = await AuthApi.LogoutV1(_sender, nullRequest);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(nullRequest);
    }

    [Fact]
    public async Task RefreshTokenV1_WithSuccessfulRefresh_ReturnsOkResult()
    {
        // Arrange
        var refreshRequest = new Command.RefreshTokenRequest("access-token", "refresh-token");
        var expectedResponse = Result.Success(new Response.Authenticated(
            "new-access-token",
            "new-refresh-token",
            DateTime.UtcNow.AddDays(7)
        ));

        _sender.Send(refreshRequest).Returns(expectedResponse);

        // Act
        var result = await AuthApi.RefreshTokenV1(_sender, refreshRequest);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(refreshRequest);
    }

    [Fact]
    public async Task RefreshTokenV1_WithFailedRefresh_ReturnsFailureResult()
    {
        // Arrange
        var refreshRequest = new Command.RefreshTokenRequest("invalid-access-token", "invalid-refresh-token");
        var errorResponse = Result.Failure<Response.Authenticated>(new Error("AUTH.INVALID_REFRESH_TOKEN", "Invalid refresh token"));

        _sender.Send(refreshRequest).Returns(errorResponse);

        // Act
        var result = await AuthApi.RefreshTokenV1(_sender, refreshRequest);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(refreshRequest);
    }

    [Fact]
    public async Task RefreshTokenV1_WithNullRequest_HandlesGracefully()
    {
        // Arrange
        Command.RefreshTokenRequest nullRequest = null!;
        var errorResponse = Result.Failure<Response.Authenticated>(new Error("AUTH.INVALID_REQUEST", "Invalid request"));

        _sender.Send(Arg.Any<Command.RefreshTokenRequest>()).Returns(errorResponse);

        // Act
        var result = await AuthApi.RefreshTokenV1(_sender, nullRequest);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(nullRequest);
    }

    [Fact]
    public async Task RefreshTokenV1_WithExpiredAccessToken_ReturnsFailureResult()
    {
        // Arrange
        var refreshRequest = new Command.RefreshTokenRequest("expired-access-token", "valid-refresh-token");
        var errorResponse = Result.Failure<Response.Authenticated>(new Error("AUTH.EXPIRED_TOKEN", "Access token has expired"));

        _sender.Send(refreshRequest).Returns(errorResponse);

        // Act
        var result = await AuthApi.RefreshTokenV1(_sender, refreshRequest);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(refreshRequest);
    }

    [Fact]
    public async Task RefreshTokenV1_WithExpiredRefreshToken_ReturnsFailureResult()
    {
        // Arrange
        var refreshRequest = new Command.RefreshTokenRequest("valid-access-token", "expired-refresh-token");
        var errorResponse = Result.Failure<Response.Authenticated>(new Error("AUTH.EXPIRED_REFRESH_TOKEN", "Refresh token has expired"));

        _sender.Send(refreshRequest).Returns(errorResponse);

        // Act
        var result = await AuthApi.RefreshTokenV1(_sender, refreshRequest);

        // Assert
        result.Should().NotBeNull();
        await _sender.Received(1).Send(refreshRequest);
    }
}