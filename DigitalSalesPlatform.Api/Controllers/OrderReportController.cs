using AutoMapper;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSalesPlatform.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderReportController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrderReportController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpGet("report")]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<List<OrderReportResponse>>> GetOrderReport()
    {
        var query = new GetOrderReportQuery();
        var response = await _mediator.Send(query);
        return response;
    }
}