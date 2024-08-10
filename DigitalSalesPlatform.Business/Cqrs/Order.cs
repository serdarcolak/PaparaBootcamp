using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Cqrs;

public record CreateCreditCardOrderCommand(OrderCreditCardRequest Request) : IRequest<ApiResponse<OrderResponse>>;
public record CreateOrderDigitalWalletCommand(OrderDigitalWalletRequest Request) : IRequest<ApiResponse<OrderResponse>>;
public record DeleteOrderCommand(int OrderId) : IRequest<ApiResponse>;
public record GetAllOrdersQuery() : IRequest<ApiResponse<List<OrderResponse>>>;
public record GetPastOrdersQuery() : IRequest<ApiResponse<List<OrderResponse>>>;

public record GetActiveOrdersQuery() : IRequest<ApiResponse<List<OrderResponse>>>;
public record GetOrderByIdQuery(int OrderId) : IRequest<ApiResponse<OrderResponse>>;
