using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Cqrs;

public record CreateProductCommand(ProductRequest Request) : IRequest<ApiResponse<ProductResponse>>;
public record UpdateProductCommand(int ProductId, ProductRequest Request) : IRequest<ApiResponse>;
public record DeleteProductCommand(int ProductId) : IRequest<ApiResponse>;
public record GetAllProductsQuery() : IRequest<ApiResponse<List<ProductResponse>>>;
public record GetProductByIdQuery(int ProductId) : IRequest<ApiResponse<ProductResponse>>;
public record GetProductsByCategoryQuery(int CategoryId) : IRequest<ApiResponse<List<ProductResponse>>>;

public record UpdateStockCommand(int ProductId, int Stock) : IRequest<ApiResponse>;