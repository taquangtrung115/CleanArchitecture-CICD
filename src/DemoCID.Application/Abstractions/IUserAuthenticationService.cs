using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Application.Abstractions;

public interface IUserAuthenticationService
{
    Task<UserAuthResult> ValidateUserAsync(string userName, string password);
    Task<IEnumerable<string>> GetUserRolesAsync(string userId);
}

public class UserAuthResult
{
    public bool IsSuccess { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? ErrorMessage { get; set; }

    public static UserAuthResult Success(string userId, string userName, string? email = null, string? fullName = null)
    {
        return new UserAuthResult
        {
            IsSuccess = true,
            UserId = userId,
            UserName = userName,
            Email = email,
            FullName = fullName
        };
    }

    public static UserAuthResult Failure(string errorMessage)
    {
        return new UserAuthResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}