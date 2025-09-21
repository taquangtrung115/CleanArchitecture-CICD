using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

public sealed class ForgotPasswordCommandHandler : ICommandHandler<Command.ForgotPassword>
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public ForgotPasswordCommandHandler(
        IUserAuthenticationService userAuthenticationService)
    {
        _userAuthenticationService = userAuthenticationService;
    }

    public async Task<Result> Handle(Command.ForgotPassword request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userAuthenticationService.SendForgotPasswordEmailAsync(request.Email);

            if (!result)
            {
                // For security reasons, we still return success even if email doesn't exist
                // This prevents email enumeration attacks
                Log.Warning("Forgot password attempt for non-existent email: {Email}", request.Email);
            }
            else
            {
                Log.Information("Forgot password email sent successfully for email: {Email}", request.Email);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during forgot password for email: {Email}", request.Email);
            return Result.Failure(new Error("ForgotPassword.Error", "An error occurred while processing your request"));
        }
    }
}