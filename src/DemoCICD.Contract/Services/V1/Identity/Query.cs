using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using MediatR;

namespace DemoCICD.Contract.Services.V1.Identity;

public static class Query
{
    public record Login(string UserName, string Password) : IQuery<Response.Authenticated>;
    public record Token(string? AccessToken, string? RefreshToken) : IQuery<Response.Authenticated>;
}
