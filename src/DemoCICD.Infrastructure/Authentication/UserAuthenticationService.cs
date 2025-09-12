using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Application.Abstractions;
using DemoCICD.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Infrastructure.Authentication;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<UserAuthenticationService> _logger;

    public UserAuthenticationService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ILogger<UserAuthenticationService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<UserAuthResult> ValidateUserAsync(string userName, string password)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                _logger.LogWarning("Login attempt with invalid username: {UserName}", userName);
                return UserAuthResult.Failure("Invalid username or password");
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                _logger.LogWarning("Login attempt with invalid password for user: {UserId}", user.Id);
                return UserAuthResult.Failure("Invalid username or password");
            }

            return UserAuthResult.Success(
                user.Id.ToString(),
                user.UserName ?? string.Empty,
                user.Email,
                user.FullName
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user credentials for username: {UserName}", userName);
            return UserAuthResult.Failure("An error occurred during authentication");
        }
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Enumerable.Empty<string>();
            }

            return await _userManager.GetRolesAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user roles for user: {UserId}", userId);
            return Enumerable.Empty<string>();
        }
    }

    public async Task<UserAuthResult> RegisterUserAsync(string userName, string email, string password, string firstName, string lastName, DateTime? dayOfBirth)
    {
        try
        {
            var existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser != null)
            {
                return UserAuthResult.Failure("Username already exists");
            }

            existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return UserAuthResult.Failure("Email already exists");
            }

            var user = new AppUser
            {
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                FullName = $"{firstName} {lastName}",
                DayOfBirth = dayOfBirth,
                EmailConfirmed = true // Auto-confirm for simplicity
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return UserAuthResult.Failure($"User creation failed: {errors}");
            }

            _logger.LogInformation("User {UserName} registered successfully with ID {UserId}", userName, user.Id);

            return UserAuthResult.Success(
                user.Id.ToString(),
                user.UserName,
                user.Email,
                user.FullName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {UserName}", userName);
            return UserAuthResult.Failure("An error occurred during registration");
        }
    }
}