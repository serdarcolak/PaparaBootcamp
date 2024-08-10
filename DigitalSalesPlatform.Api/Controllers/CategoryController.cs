using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSalesPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<CategoryResponse>> CreateCategory([FromBody] CategoryRequest request)
    {
        var command = new CreateCategoryCommand(request);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpPut("{categoryId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> UpdateCategory(int categoryId, [FromBody] CategoryRequest request)
    {
        var command = new UpdateCategoryCommand(categoryId, request);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpDelete("{categoryId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> DeleteCategory(int categoryId)
    {
        var command = new DeleteCategoryCommand(categoryId);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpGet]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<List<CategoryResponse>>> GetAllCategories()
    {
        var categories = new GetAllCategoriesQuery();
        var result = await _mediator.Send(categories);
        return result;
    }
}
