using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema;

public class OrderCreditCardRequest : BaseRequest
{
    public int UserId { get; set; }
    public List<OrderDetailRequest> OrderDetails { get; set; }
    public string CouponCode { get; set; }
    public string CardNumber { get; set; }
    public string ExpiryDate { get; set; }
    public string CVC { get; set; }
}