using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Cqrs;

public record CreateCategoryCommand(CategoryRequest Request) : IRequest<ApiResponse<CategoryResponse>>;
public record UpdateCategoryCommand(int CategoryId, CategoryRequest Request) : IRequest<ApiResponse>;
public record DeleteCategoryCommand(int CategoryId) : IRequest<ApiResponse>;

public record GetAllCategoriesQuery() : IRequest<ApiResponse<List<CategoryResponse>>>;
