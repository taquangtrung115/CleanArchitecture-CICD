using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Contract.Services.V1.Identity;

public static class Response
{
    public record Authenticated(string? AccessToken, string? RefreshToken, DateTime? RefreshTokenExpiryTime);
}
