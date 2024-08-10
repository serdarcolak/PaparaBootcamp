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
public class CouponController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CouponController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<CouponResponse>>  CreateCoupon([FromBody] CouponRequest request)
    {
        var command = new CreateCouponCommand(request);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpPut("{couponId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> UpdateCoupon(int couponId, [FromBody] CouponRequest request)
    {
        var command = new UpdateCouponCommand(couponId, request);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpDelete("{couponId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> DeleteCoupon(int couponId)
    {
        var command = new DeleteCouponCommand(couponId);
        var response = await _mediator.Send(command);
        return response;
    }
    
    [HttpGet("unused")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<CouponResponse>>> GetUnusedCoupons()
    {
        var query = new GetUnusedCouponsQuery();
        var response = await _mediator.Send(query);
        return response;
    }

    [HttpGet("used")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<CouponResponse>>> GetUsedCoupons()
    {
        var query = new GetUsedCouponsQuery();
        var response = await _mediator.Send(query);
        return response;
    }
}