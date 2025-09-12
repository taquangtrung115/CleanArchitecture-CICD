using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;

namespace DemoCICD.Contract.Services.V1.Identity;

public static class Command
{
    public record Logout(string AccessToken) : ICommand;
    public record RefreshTokenRequest(string AccessToken, string RefreshToken) : ICommand<Response.Authenticated>;
}