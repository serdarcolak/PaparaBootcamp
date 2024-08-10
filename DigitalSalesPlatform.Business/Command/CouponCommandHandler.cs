using AutoMapper;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Business.Service;
using DigitalSalesPlatform.Data;
using MediatR;
using Newtonsoft.Json;

namespace DigitalSalesPlatform.Business.Command;

public class CouponCommandHandler:
    IRequestHandler<CreateCouponCommand, ApiResponse<CouponResponse>>,
    IRequestHandler<UpdateCouponCommand, ApiResponse>,
    IRequestHandler<DeleteCouponCommand, ApiResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRedisCacheService _redisCacheService;

    public CouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IRedisCacheService redisCacheService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisCacheService = redisCacheService;
    }

    public async Task<ApiResponse<CouponResponse>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = _mapper.Map<Coupon>(request.Request);
        await _unitOfWork.CouponRepository.Insert(coupon);
        await _unitOfWork.Complete();
        var couponResponse = _mapper.Map<CouponResponse>(coupon);
        await _redisCacheService.SetValueAsync($"coupon_{coupon.Id}", JsonConvert.SerializeObject(couponResponse));
        return new ApiResponse<CouponResponse>(couponResponse);
    }

    public async Task<ApiResponse> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _unitOfWork.CouponRepository.GetById(request.CouponId);
        if (coupon == null)
        {
            return new ApiResponse("Coupon not found");
        }

        _mapper.Map(request.Request, coupon);
        _unitOfWork.CouponRepository.Update(coupon);
        await _unitOfWork.Complete();
        
        var updatedCouponResponse = _mapper.Map<CouponResponse>(coupon);
        await _redisCacheService.SetValueAsync($"coupon_{coupon.Id}", JsonConvert.SerializeObject(updatedCouponResponse));
        
        return new ApiResponse("Coupon updated successfully");
    }

    public async Task<ApiResponse> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.CouponRepository.Delete(request.CouponId);
        await _unitOfWork.Complete();
        await _redisCacheService.Clear($"coupon_{request.CouponId}");
        return new ApiResponse("Coupon deleted successfully");
    }
}