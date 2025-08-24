

using Asp.Versioning;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V2.Product;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoCICD.Presentation.Controllers.V2;

[ApiVersion(2)]
public class ProductsController : ApiController
{
    public ProductsController(ISender sender) : base(sender)
    {
    }

    [HttpPost(Name = "CreateProducts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Products([FromBody] Command.CreateProductCommand CreateProduct)
    {
        var result = await Sender.Send(CreateProduct);
        return Ok(result);
    }

    //[HttpGet(Name = "GetProducts")]
    //[ProducesResponseType(typeof(Result<IEnumerable<Response.ProductResponse>>), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> Products(string? serchTerm = null,
    //    string? sortColumn = null,
    //    string? sortOrder = null,
    //    string? sortColumnAndOrder = null,
    //    int pageIndex = 1,
    //    int pageSize = 10)
    //{
    //    //var sort = !string.IsNullOrWhiteSpace(sortOrder)
    //    //    ? sortOrder.Equals("Asc")
    //    //    ? SortOrder.Ascending : SortOrder.Descending : SortOrder.Descending;

    //    var result = await Sender.Send(new Query.GetProductsQuery(serchTerm, sortColumn,
    //        SortOrderExtension.ConvertStringToSortOrder(sortOrder),
    //        SortOrderExtension.ConvertStringToSortOrderV2(sortColumnAndOrder),
    //        pageIndex,
    //        pageSize));
    //    return Ok(result);
    //}

    [HttpGet(Name = "GetProducts")]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.ProductResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Products()
    {
        var result = await Sender.Send(new Query.GetProductsQuery());
        return Ok(result);
    }

    [HttpGet("{productId}")]
    [ProducesResponseType(typeof(Result<Response.ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Products(Guid productId)
    {
        var result = await Sender.Send(new Query.GetProductByIdQuery(productId));
        return Ok(result);
    }

    [HttpDelete("{productId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProducts(Guid productId)
    {
        var result = await Sender.Send(new Command.DeleteProductCommand(productId));
        return Ok(result);
    }

    [HttpPut("{productId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Products(Guid productId, [FromBody] Command.UpdateProductCommand updateProduct)
    {
        var updateProductCommand = new Command.UpdateProductCommand(productId, updateProduct.Name, updateProduct.Price, updateProduct.Description);
        var result = await Sender.Send(updateProductCommand);
        return Ok(result);
    }
}
