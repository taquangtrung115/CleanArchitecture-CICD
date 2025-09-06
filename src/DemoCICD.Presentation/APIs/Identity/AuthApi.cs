using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carter;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.Identity;

public class AuthApi :  ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/carter/v{version:apiVersion}/auth";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Authentication")
            .MapGroup(BaseUrl).HasApiVersion(1).RequireAuthorization();

        group1.MapPost("login", AuthenticationV1).AllowAnonymous();

        //var group2 = app.NewVersionedApi("auth-cater-name-show-on-swagger")
        //    .MapGroup(BaseUrl).HasApiVersion(2);

        //group2.MapPost(string.Empty, CreateProductsV2);
        //group2.MapGet(string.Empty, GetProductsV2);
        //group2.MapGet("{productId}", GetProductsByIdV2);
        //group2.MapDelete("{productId}", DeleteProductsV2);
        //group2.MapPut("{productId}", UpdateProductsV2);
    }
    public static async Task<IResult> AuthenticationV1(ISender sender, [FromBody] DemoCICD.Contract.Services.V1.Identity.Query.Login login)
    {
        var result = await sender.Send(login);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}
