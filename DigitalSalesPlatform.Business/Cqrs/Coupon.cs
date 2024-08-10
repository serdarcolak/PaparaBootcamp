using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Cqrs;

public record CreateCouponCommand(CouponRequest Request) : IRequest<ApiResponse<CouponResponse>>;
public record UpdateCouponCommand(int CouponId,CouponRequest Request) : IRequest<ApiResponse>;
public record DeleteCouponCommand(int CouponId) : IRequest<ApiResponse>;
public record GetUnusedCouponsQuery() : IRequest<ApiResponse<List<CouponResponse>>>;
public record GetUsedCouponsQuery() : IRequest<ApiResponse<List<CouponResponse>>>;