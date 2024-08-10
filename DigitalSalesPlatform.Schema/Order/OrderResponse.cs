using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema;

public class OrderResponse : BaseResponse
{
    public int UserId { get; set; }
    public int OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal WalletUsed { get; set; }
    public string CouponCode { get; set; }
    public decimal CouponAmount { get; set; }
    public decimal PointsUsed { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderDetailResponse> OrderDetails { get; set; }
}