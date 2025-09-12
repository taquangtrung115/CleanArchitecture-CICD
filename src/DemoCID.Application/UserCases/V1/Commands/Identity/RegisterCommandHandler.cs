using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

public class RegisterCommandHandler : ICommandHandler<Command.Register, Response.UserCreated>
{
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IUserAuthenticationService userAuthenticationService,
        ILogger<RegisterCommandHandler> logger)
    {
        _userAuthenticationService = userAuthenticationService;
        _logger = logger;
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

            _logger.LogInformation("User {UserName} registered successfully with ID {UserId}", request.UserName, result.UserId);

            var response = new Response.UserCreated(
                Guid.Parse(result.UserId!),
                result.UserName!,
                result.Email!);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for user: {UserName}", request.UserName);
            return Result.Failure<Response.UserCreated>(
                new Error("Registration.Error", "An error occurred during registration"));
        }
    }
}