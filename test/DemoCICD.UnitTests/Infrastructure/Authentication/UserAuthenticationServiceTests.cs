using DemoCICD.Application.Abstractions;
using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Infrastructure.Authentication;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace DemoCICD.UnitTests.Infrastructure.Authentication;

public class UserAuthenticationServiceTests
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<UserAuthenticationService> _logger;
    private readonly UserAuthenticationService _userAuthenticationService;

    public UserAuthenticationServiceTests()
    {
        _userManager = Substitute.For<UserManager<AppUser>>(
            Substitute.For<IUserStore<AppUser>>(),
            null, null, null, null, null, null, null, null);
        _signInManager = Substitute.For<SignInManager<AppUser>>(
            _userManager,
            Substitute.For<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
            Substitute.For<IUserClaimsPrincipalFactory<AppUser>>(),
            null, null, null, null);
        _logger = Substitute.For<ILogger<UserAuthenticationService>>();
        _userAuthenticationService = new UserAuthenticationService(_userManager, _signInManager, _logger);
    }

    [Fact]
    public async Task ValidateUserAsync_WithValidCredentials_ReturnsSuccessResult()
    {
        // Arrange
        var userName = "testuser";
        var password = "password123";
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            Email = "test@example.com",
            FullName = "Test User"
        };

        _userManager.FindByNameAsync(userName).Returns(user);
        _signInManager.CheckPasswordSignInAsync(user, password, false).Returns(SignInResult.Success);

        // Act
        var result = await _userAuthenticationService.ValidateUserAsync(userName, password);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.UserId.Should().Be(user.Id.ToString());
        result.UserName.Should().Be(user.UserName);
        result.Email.Should().Be(user.Email);
        result.FullName.Should().Be(user.FullName);
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public async Task ValidateUserAsync_WithInvalidUserName_ReturnsFailureResult()
    {
        // Arrange
        var userName = "nonexistentuser";
        var password = "password123";

        _userManager.FindByNameAsync(userName).ReturnsNull();

        // Act
        var result = await _userAuthenticationService.ValidateUserAsync(userName, password);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid username or password");
        result.UserId.Should().BeNull();
        result.UserName.Should().BeNull();
        result.Email.Should().BeNull();
        result.FullName.Should().BeNull();
    }

    [Fact]
    public async Task ValidateUserAsync_WithInvalidPassword_ReturnsFailureResult()
    {
        // Arrange
        var userName = "testuser";
        var password = "wrongpassword";
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            Email = "test@example.com",
            FullName = "Test User"
        };

        _userManager.FindByNameAsync(userName).Returns(user);
        _signInManager.CheckPasswordSignInAsync(user, password, false).Returns(SignInResult.Failed);

        // Act
        var result = await _userAuthenticationService.ValidateUserAsync(userName, password);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid username or password");
        result.UserId.Should().BeNull();
        result.UserName.Should().BeNull();
        result.Email.Should().BeNull();
        result.FullName.Should().BeNull();
    }

    [Fact]
    public async Task ValidateUserAsync_WhenExceptionThrown_ReturnsFailureResult()
    {
        // Arrange
        var userName = "testuser";
        var password = "password123";

        _userManager.When(x => x.FindByNameAsync(userName))
            .Do(x => throw new Exception("Database error"));

        // Act
        var result = await _userAuthenticationService.ValidateUserAsync(userName, password);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("An error occurred during authentication");
        result.UserId.Should().BeNull();
        result.UserName.Should().BeNull();
        result.Email.Should().BeNull();
        result.FullName.Should().BeNull();
    }

    [Fact]
    public async Task GetUserRolesAsync_WithValidUserId_ReturnsUserRoles()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new AppUser { Id = Guid.Parse(userId), UserName = "testuser" };
        var expectedRoles = new List<string> { "Admin", "User" };

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.GetRolesAsync(user).Returns(expectedRoles);

        // Act
        var result = await _userAuthenticationService.GetUserRolesAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedRoles);
    }

    [Fact]
    public async Task GetUserRolesAsync_WithInvalidUserId_ReturnsEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();

        _userManager.FindByIdAsync(userId).ReturnsNull();

        // Act
        var result = await _userAuthenticationService.GetUserRolesAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUserRolesAsync_WhenExceptionThrown_ReturnsEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();

        _userManager.When(x => x.FindByIdAsync(userId))
            .Do(x => throw new Exception("Database error"));

        // Act
        var result = await _userAuthenticationService.GetUserRolesAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}