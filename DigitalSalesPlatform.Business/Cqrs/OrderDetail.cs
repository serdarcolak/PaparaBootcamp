using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Cqrs;

public record CreateOrderDetailCommand(OrderDetailRequest Request) : IRequest<ApiResponse<OrderDetailResponse>>;
public record UpdateOrderDetailCommand(int OrderDetailId,OrderDetailRequest Request) : IRequest<ApiResponse>;
public record DeleteOrderDetailCommand(int OrderDetailId) : IRequest<ApiResponse>;

public record GetAllOrderDetailQuery() : IRequest<ApiResponse<List<OrderDetailResponse>>>;
public record GetOrderDetailByIdQuery(int OrderDetailId) : IRequest<ApiResponse<OrderDetailResponse>>;