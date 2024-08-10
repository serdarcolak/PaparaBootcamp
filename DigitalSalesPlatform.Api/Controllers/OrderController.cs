using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSalesPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("order/creditCard")]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<OrderResponse>>  CreateOrderCreditCard([FromBody] OrderCreditCardRequest request)
    {
        var command = new CreateCreditCardOrderCommand(request);
        var response = await _mediator.Send(command);
        return response;
    }
    
    [HttpPost("order/digitalWallet")]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<OrderResponse>>  CreateOrderDigitalWallet([FromBody] OrderDigitalWalletRequest request)
    {
        var command = new CreateOrderDigitalWalletCommand(request);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpDelete("{orderId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse>DeleteOrder(int orderId)
    {
        var command = new DeleteOrderCommand(orderId);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<OrderResponse>>> GetAllOrders()
    {
        var query = new GetAllOrdersQuery();
        var response = await _mediator.Send(query);
        return response;
    }
    
    [HttpGet("orders/pasts")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<OrderResponse>>> GetPastOrders()
    {
        var query = new GetPastOrdersQuery();
        var response = await _mediator.Send(query);
        return response;
    }
    
    [HttpGet("orders/actives")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<OrderResponse>>> GetActiveOrders()
    {
        var query = new GetActiveOrdersQuery();
        var response = await _mediator.Send(query);
        return response;
    }

    [HttpGet("{orderId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<OrderResponse>>  GetOrderById(int orderId)
    {
        var query = new GetOrderByIdQuery(orderId);
        var response = await _mediator.Send(query);
        return response;
    }
}
