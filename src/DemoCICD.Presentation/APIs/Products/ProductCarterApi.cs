

using Carter;
using DemoCICD.Contract.Extensions;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using CommandV1 = DemoCICD.Contract.Services.V1.Product;
using CommandV2 = DemoCICD.Contract.Services.V2.Product;

namespace DemoCICD.Presentation.APIs.Products;

public class ProductCarterApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/carter/v{version:apiVersion}/products";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("products-cater-name-show-on-swagger")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost(string.Empty, CreateProductsV1);
        group1.MapGet(string.Empty, GetProductsV1);
        group1.MapGet("{productId}", GetProductsByIdV1);
        group1.MapDelete("{productId}", DeleteProductsV1);
        group1.MapPut("{productId}", UpdateProductsV1);

        var group2 = app.NewVersionedApi("products-cater-name-show-on-swagger")
            .MapGroup(BaseUrl).HasApiVersion(2);

        group2.MapPost(string.Empty, CreateProductsV2);
        group2.MapGet(string.Empty, GetProductsV2);
        group2.MapGet("{productId}", GetProductsByIdV2);
        group2.MapDelete("{productId}", DeleteProductsV2);
        group2.MapPut("{productId}", UpdateProductsV2);
    }

    #region ====== version 1 ======

    public static async Task<IResult> CreateProductsV1(ISender sender, [FromBody] CommandV1.Command.CreateProductCommand CreateProduct)
    {
        var result = await sender.Send(CreateProduct);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetProductsV1(ISender sender, string? serchTerm = null,
        string? sortColumn = null,
        string? sortOrder = null,
        string? sortColumnAndOrder = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var result = await sender.Send(new CommandV1.Query.GetProductsQuery(serchTerm, sortColumn,
            SortOrderExtension.ConvertStringToSortOrder(sortOrder),
            SortOrderExtension.ConvertStringToSortOrderV2(sortColumnAndOrder),
            pageIndex,
            pageSize));
        return Results.Ok(result);
    }

    public static async Task<IResult> GetProductsByIdV1(ISender sender, Guid productId)
    {
        var result = await sender.Send(new CommandV1.Query.GetProductByIdQuery(productId));
        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteProductsV1(ISender sender, Guid productId)
    {
        var result = await sender.Send(new CommandV1.Command.DeleteProductCommand(productId));
        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateProductsV1(ISender sender, Guid productId, [FromBody] CommandV1.Command.UpdateProductCommand updateProduct)
    {
        var updateProductCommand = new CommandV1.Command.UpdateProductCommand(productId, updateProduct.Name, updateProduct.Price, updateProduct.Description);
        var result = await sender.Send(updateProductCommand);
        return Results.Ok(result);
    }

    #endregion ====== version 1 ======

    #region ====== version 2 ======

    public static async Task<IResult> CreateProductsV2(ISender sender, [FromBody] CommandV2.Command.CreateProductCommand CreateProduct)
    {
        var result = await sender.Send(CreateProduct);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetProductsV2(ISender sender)
    {
        var result = await sender.Send(new CommandV2.Query.GetProductsQuery());
        return Results.Ok(result);
    }

    public static async Task<IResult> GetProductsByIdV2(ISender sender, Guid productId)
    {
        var result = await sender.Send(new CommandV2.Query.GetProductByIdQuery(productId));
        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteProductsV2(ISender sender, Guid productId)
    {
        var result = await sender.Send(new CommandV2.Command.DeleteProductCommand(productId));
        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateProductsV2(ISender sender, Guid productId, [FromBody] CommandV2.Command.UpdateProductCommand updateProduct)
    {
        var updateProductCommand = new CommandV2.Command.UpdateProductCommand(productId, updateProduct.Name, updateProduct.Price, updateProduct.Description);
        var result = await sender.Send(updateProductCommand);
        return Results.Ok(result);
    }

    #endregion ====== version 2 ======
}
