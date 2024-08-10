using AutoMapper;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Query;

public class OrderQueryHandler :
    IRequestHandler<GetAllOrdersQuery, ApiResponse<List<OrderResponse>>>,
    IRequestHandler<GetOrderByIdQuery, ApiResponse<OrderResponse>>,
    IRequestHandler<GetPastOrdersQuery, ApiResponse<List<OrderResponse>>>,
    IRequestHandler<GetActiveOrdersQuery, ApiResponse<List<OrderResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<OrderResponse>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _unitOfWork.OrderRepository.GetAll("OrderDetails", "OrderDetails.Product");
        var mapped = _mapper.Map<List<OrderResponse>>(orders);
        return new ApiResponse<List<OrderResponse>>(mapped);
    }

    public async Task<ApiResponse<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.OrderRepository.GetById(request.OrderId, "OrderDetails", "OrderDetails.Product");
        var mapped = _mapper.Map<OrderResponse>(order);
        return new ApiResponse<OrderResponse>(mapped);
    }

    public async Task<ApiResponse<List<OrderResponse>>> Handle(GetPastOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _unitOfWork.OrderRepository.Where(o => o.OrderDate.Date < DateTime.UtcNow.Date);
        var mapped = _mapper.Map<List<OrderResponse>>(orders);
        return new ApiResponse<List<OrderResponse>>(mapped);
    }

    public async Task<ApiResponse<List<OrderResponse>>> Handle(GetActiveOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _unitOfWork.OrderRepository.Where(o => o.OrderDate.Date >= DateTime.UtcNow.Date);
        var mapped = _mapper.Map<List<OrderResponse>>(orders);
        return new ApiResponse<List<OrderResponse>>(mapped);
    }
}
