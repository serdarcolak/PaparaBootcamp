using AutoMapper;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Data;
using MediatR;

namespace DigitalSalesPlatform.Business.Query;

public class CouponQueryHandler : 
    IRequestHandler<GetUsedCouponsQuery, ApiResponse<List<CouponResponse>>>,
    IRequestHandler<GetUnusedCouponsQuery, ApiResponse<List<CouponResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CouponQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ApiResponse<List<CouponResponse>>> Handle(GetUsedCouponsQuery request, CancellationToken cancellationToken)
    {
        var coupons = await _unitOfWork.CouponRepository.Where(c => !c.IsActive || c.ExpiryDate < DateTime.UtcNow);
        var mapped = _mapper.Map<List<CouponResponse>>(coupons);
        return new ApiResponse<List<CouponResponse>>(mapped);
    }
    
    public async Task<ApiResponse<List<CouponResponse>>> Handle(GetUnusedCouponsQuery request, CancellationToken cancellationToken)
    {
        var coupons = await _unitOfWork.CouponRepository.Where(c => c.IsActive && c.ExpiryDate >= DateTime.UtcNow);
        var mappedCoupons = _mapper.Map<List<CouponResponse>>(coupons);
        return new ApiResponse<List<CouponResponse>>(mappedCoupons);
    }
}