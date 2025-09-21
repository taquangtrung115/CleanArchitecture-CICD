using DemoCICD.Application.Abstractions;
using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DemoCICD.Infrastructure.Authentication;

public class UserManagementService : IUserManagementService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public UserManagementService(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        ApplicationDbContext context,
        IEmailService emailService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _emailService = emailService;
    }

    public async Task<AppUser?> GetUserByIdAsync(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<(IEnumerable<AppUser> Users, int TotalCount)> GetUsersAsync(int page, int pageSize, string? searchTerm)
    {
        var query = _userManager.Users.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(u => u.UserName!.Contains(searchTerm) ||
                                   u.Email!.Contains(searchTerm) ||
                                   u.FirstName.Contains(searchTerm) ||
                                   u.LastName.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }

    public async Task<UserAuthResult> CreateUserAsync(string userName, string email, string password, string firstName, string lastName, DateTime? dayOfBirth, bool? isDirector, bool? isHeadOfDepartment, Guid? managerId, Guid positionId)
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
                IsDirector = isDirector,
                IsHeadOfDepartment = isHeadOfDepartment,
                ManagerId = managerId,
                PositionId = positionId,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return UserAuthResult.Failure($"User creation failed: {errors}");
            }

            return UserAuthResult.Success(user.Id.ToString(), user.UserName, user.Email, user.FullName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating user: {UserName}", userName);
            return UserAuthResult.Failure("An error occurred during user creation");
        }
    }

    public async Task<bool> UpdateUserAsync(Guid userId, string email, string firstName, string lastName, DateTime? dayOfBirth, bool? isDirector, bool? isHeadOfDepartment, Guid? managerId, Guid positionId, string? phone = null, string? address = null, string? city = null, string? country = null, string? bio = null, string? website = null)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.FullName = $"{firstName} {lastName}";
            user.DayOfBirth = dayOfBirth;
            user.IsDirector = isDirector;
            user.IsHeadOfDepartment = isHeadOfDepartment;
            user.ManagerId = managerId;
            user.PositionId = positionId;
            user.Phone = phone;
            user.Address = address;
            user.City = city;
            user.Country = country;
            user.Bio = bio;
            user.Website = website;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> UpdateProfileAsync(Guid userId, string firstName, string lastName, string? phone = null, string? address = null, string? city = null, string? country = null, string? bio = null, string? website = null)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            user.FirstName = firstName;
            user.LastName = lastName;
            user.FullName = $"{firstName} {lastName}";
            user.Phone = phone;
            user.Address = address;
            user.City = city;
            user.Country = country;
            user.Bio = bio;
            user.Website = website;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating profile for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error changing password for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> ResetPasswordAsync(Guid userId, string newPassword)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error resetting password for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> LockUserAsync(Guid userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error locking user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> UnlockUserAsync(Guid userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error unlocking user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> AssignUserToRoleAsync(Guid userId, Guid roleId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name!);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error assigning user {UserId} to role {RoleId}", userId, roleId);
            return false;
        }
    }

    public async Task<bool> RemoveUserFromRoleAsync(Guid userId, Guid roleId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return false;
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name!);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error removing user {UserId} from role {RoleId}", userId, roleId);
            return false;
        }
    }

    public async Task<IEnumerable<AppRole>> GetUserRolesAsync(Guid userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Enumerable.Empty<AppRole>();
            }

            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = new List<AppRole>();

            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    roles.Add(role);
                }
            }

            return roles;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting roles for user: {UserId}", userId);
            return Enumerable.Empty<AppRole>();
        }
    }

    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _userManager.FindByEmailAsync(email);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting user by email: {Email}", email);
            return null;
        }
    }

    public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
    {
        try
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error generating password reset token for user: {UserId}", user.Id);
            throw;
        }
    }

    public async Task<bool> ResetPasswordWithTokenAsync(string email, string token, string newPassword)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error resetting password with token for email: {Email}", email);
            return false;
        }
    }
    // Password Reset Methods
    public async Task<bool> InitiatePasswordResetAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // For security, don't reveal if email exists
                Log.Warning("Password reset requested for non-existent email: {Email}", email);
                return false;
            }

            // Generate a 6-digit reset code
            var resetCode = GenerateResetCode();
            
            // Clean up old reset tokens for this email
            var oldTokens = await _context.PasswordResetTokens
                .Where(t => t.Email == email && !t.IsUsed)
                .ToListAsync();
                
            foreach (var oldToken in oldTokens)
            {
                oldToken.IsUsed = true;
                oldToken.UsedAt = DateTime.UtcNow;
            }

            // Create new reset token
            var resetToken = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                Email = email,
                ResetCode = resetCode,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15), // 15 minute expiry
                CreatedAt = DateTime.UtcNow,
                IsUsed = false
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            // Send email with reset code
            var emailSent = await _emailService.SendPasswordResetCodeAsync(email, resetCode, user.FirstName);
            
            if (!emailSent)
            {
                Log.Error("Failed to send password reset email to: {Email}", email);
                return false;
            }

            Log.Information("Password reset code sent to: {Email}", email);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error initiating password reset for email: {Email}", email);
            return false;
        }
    }

    public async Task<bool> VerifyResetCodeAsync(string email, string resetCode)
    {
        try
        {
            var token = await _context.PasswordResetTokens
                .Where(t => t.Email == email && t.ResetCode == resetCode && t.IsValid)
                .FirstOrDefaultAsync();

            return token != null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error verifying reset code for email: {Email}", email);
            return false;
        }
    }

    public async Task<bool> ResetPasswordWithCodeAsync(string email, string resetCode, string newPassword)
    {
        try
        {
            var token = await _context.PasswordResetTokens
                .Where(t => t.Email == email && t.ResetCode == resetCode && t.IsValid)
                .FirstOrDefaultAsync();

            if (token == null)
            {
                Log.Warning("Invalid or expired reset code used for email: {Email}", email);
                return false;
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Log.Error("User not found for email during password reset: {Email}", email);
                return false;
            }

            // Reset password using Identity
            var resetTokenCore = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetTokenCore, newPassword);

            if (result.Succeeded)
            {
                // Mark token as used
                token.IsUsed = true;
                token.UsedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                Log.Information("Password reset successful for email: {Email}", email);
                return true;
            }
            else
            {
                Log.Error("Password reset failed for email: {Email}. Errors: {Errors}", 
                    email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error resetting password for email: {Email}", email);
            return false;
        }
    }

    private string GenerateResetCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString(); // 6-digit code
    }
}
