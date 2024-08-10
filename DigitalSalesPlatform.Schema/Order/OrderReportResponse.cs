namespace DigitalSalesPlatform.Schema;

public class OrderReportResponse
{
    public int OrderId { get; set; }
    public int OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    
    public decimal WalletUsed { get; set; }
    public decimal CouponAmount { get; set; }
    public decimal PointsUsed { get; set; }
    public string CouponCode { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderDetailReportResponse> OrderDetails { get; set; }
}