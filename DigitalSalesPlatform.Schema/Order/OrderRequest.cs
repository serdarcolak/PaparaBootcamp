using DigitalSalesPlatform.Base;
using DigitalSalesPlatform.Schema;

namespace DigitalSalesPlatform.Schema;

public class OrderRequest : BaseRequest
{
    public int UserId { get; set; }
    public List<OrderDetailRequest> OrderDetails { get; set; }
    public string CouponCode { get; set; }
}