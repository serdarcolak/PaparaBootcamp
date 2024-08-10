using AutoMapper;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Query;

public class OrderReportQueryHandler :
    IRequestHandler<GetOrderReportQuery, ApiResponse<List<OrderReportResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderReportQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<OrderReportResponse>>> Handle(GetOrderReportQuery request, CancellationToken cancellationToken)
    {
        var orders = await _unitOfWork.OrderRepository.GetAll("OrderDetails", "OrderDetails.Product");
        var orderReports = orders.Select(o => new OrderReportResponse
        {
            OrderId = o.Id,
            OrderNumber = o.OrderNumber,
            WalletUsed = o.WalletUsed,
            TotalAmount = o.TotalAmount,
            CouponAmount = o.CouponAmount,
            PointsUsed = o.PointsUsed,
            CouponCode = o.OrderDetails.FirstOrDefault()?.Order.CouponCode,
            OrderDate = o.OrderDate,
            OrderDetails = o.OrderDetails.Select(od => new OrderDetailReportResponse
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                Price = od.Price
            }).ToList()
        }).ToList();

        return new ApiResponse<List<OrderReportResponse>>(orderReports);
    }
}
