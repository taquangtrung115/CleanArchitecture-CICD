using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;

namespace DemoCICD.Application.UserCases.V1.Queries.Identity;

public class GetLoginQueryHandler : IQueryHandler<Query.Login, Response.Authenticated>
{
    private readonly IJwtTokenService jwtTokenService;
    public GetLoginQueryHandler(IJwtTokenService _jwtTokenService)
    {
        jwtTokenService = _jwtTokenService;
    }
    public async Task<Result<Response.Authenticated>> Handle(Query.Login request, CancellationToken cancellationToken)
    {
        //Check user


        //Generate token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, request.UserName),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var accessToken = jwtTokenService.GenerateAccessToken(claims);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        var response = new Response.Authenticated(
            accessToken,
            refreshToken,
            DateTime.Now.AddMinutes(5)
        );

        return Result.Success(response);
    }
}
