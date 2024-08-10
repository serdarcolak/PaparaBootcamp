using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema;

public class OrderDigitalWalletRequest : BaseRequest
{
    public int UserId { get; set; }
    public List<OrderDetailRequest> OrderDetails { get; set; }
    public string CouponCode { get; set; }
}