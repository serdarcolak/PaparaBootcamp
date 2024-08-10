using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Schema;
using DigitalSalesPlatform.Schema.ProductFilter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSalesPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<ProductResponse>> CreateProduct([FromBody] ProductRequest request)
    {
        var command = new CreateProductCommand(request);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpPut("{productId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> UpdateProduct(int productId, [FromBody] ProductRequest request)
    {
        var command = new UpdateProductCommand(productId, request);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpDelete("{productId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse>  DeleteProduct(int productId)
    {
        var command = new DeleteProductCommand(productId);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpPut("{productId}/stock")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> UpdateStock(int productId, [FromBody] UpdateStockRequest request)
    {
        var command = new UpdateStockCommand(productId, request.Stock);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpGet]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<List<ProductResponse>>> GetAllProducts()
    {
        var operation = new GetAllProductsQuery();
        var response = await _mediator.Send(operation);
        return response;
    }

    [HttpGet("{productId}")]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<ProductResponse>>  GetProductById(int productId)
    {
        var query = new GetProductByIdQuery(productId);
        var response = await _mediator.Send(query);
        return response;
    }

    [HttpGet("category/{categoryId}")]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<List<ProductResponse>>>  GetProductsByCategory(int categoryId)
    {
        var query = new GetProductsByCategoryQuery(categoryId);
        var response = await _mediator.Send(query);
        return response;
    }
}
