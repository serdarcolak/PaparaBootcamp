using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Schema.ProductCategory;
using MediatR;

namespace DigitalSalesPlatform.Business.Cqrs;

public record GetAllProductCategoryQuery() : IRequest<ApiResponse<List<ProductCategoryResponse>>>;
public record GetProductCategoryByIdQuery(int ProductId) : IRequest<ApiResponse<ProductCategoryResponse>>;