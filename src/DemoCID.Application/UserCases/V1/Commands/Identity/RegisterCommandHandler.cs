using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

public sealed class RegisterCommandHandler : ICommandHandler<Command.Register, Response.UserCreated>
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public RegisterCommandHandler(
        IUserAuthenticationService userAuthenticationService)
    {
        _userAuthenticationService = userAuthenticationService;
    }

    public async Task<Result<Response.UserCreated>> Handle(Command.Register request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userAuthenticationService.RegisterUserAsync(
                request.UserName,
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.DayOfBirth);

            if (!result.IsSuccess)
            {
                return Result.Failure<Response.UserCreated>(
                    new Error("Registration.Failed", result.ErrorMessage ?? "Registration failed"));
            }

            Log.Information("User {UserName} registered successfully with ID {UserId}", request.UserName, result.UserId);

            var response = new Response.UserCreated(
                Guid.Parse(result.UserId!),
                result.UserName!,
                result.Email!);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during registration for user: {UserName}", request.UserName);
            return Result.Failure<Response.UserCreated>(
                new Error("Registration.Error", "An error occurred during registration"));
        }
    }
}
