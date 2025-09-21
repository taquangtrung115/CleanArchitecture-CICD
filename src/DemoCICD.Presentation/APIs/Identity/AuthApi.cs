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

public class AuthApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/auth";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Authentication")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost("login", LoginV1).AllowAnonymous();
        group1.MapPost("register", RegisterV1).AllowAnonymous();
        group1.MapPost("forgot-password", ForgotPasswordV1).AllowAnonymous();
        group1.MapPost("reset-password", ResetPasswordWithCodeV1).AllowAnonymous();
        group1.MapPost("logout", LogoutV1).RequireAuthorization();
        group1.MapPost("refresh-token", RefreshTokenV1).AllowAnonymous();
        
        // Password Reset endpoints
        group1.MapPost("verify-reset-code", VerifyResetCodeV1).AllowAnonymous();

        //var group2 = app.NewVersionedApi("auth-cater-name-show-on-swagger")
        //    .MapGroup(BaseUrl).HasApiVersion(2);

        //group2.MapPost(string.Empty, CreateProductsV2);
        //group2.MapGet(string.Empty, GetProductsV2);
        //group2.MapGet("{productId}", GetProductsByIdV2);
        //group2.MapDelete("{productId}", DeleteProductsV2);
        //group2.MapPut("{productId}", UpdateProductsV2);
    }

    public static async Task<IResult> LoginV1(ISender sender, DemoCICD.Contract.Services.V1.Identity.Query.Login login)
    {
        var result = await sender.Send(login);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> LogoutV1(ISender sender, DemoCICD.Contract.Services.V1.Identity.Command.Logout logout)
    {
        var result = await sender.Send(logout);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> RefreshTokenV1(ISender sender, DemoCICD.Contract.Services.V1.Identity.Command.RefreshTokenRequest refreshToken)
    {
        var result = await sender.Send(refreshToken);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> RegisterV1(ISender sender, DemoCICD.Contract.Services.V1.Identity.Command.Register register)
    {
        var result = await sender.Send(register);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> ForgotPasswordV1(ISender sender, DemoCICD.Contract.Services.V1.Identity.Command.ForgotPassword forgotPassword)
    {
        var result = await sender.Send(forgotPassword);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> VerifyResetCodeV1(ISender sender, DemoCICD.Contract.Services.V1.Identity.Command.VerifyResetCode verifyResetCode)
    {
        var result = await sender.Send(verifyResetCode);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> ResetPasswordWithCodeV1(ISender sender, DemoCICD.Contract.Services.V1.Identity.Command.ResetPasswordWithCode resetPassword)
    {
        var result = await sender.Send(resetPassword);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}
