using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Cqrs;

public record GetOrderReportQuery : IRequest<ApiResponse<List<OrderReportResponse>>>;