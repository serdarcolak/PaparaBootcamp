using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema;

public class CouponRequest : BaseRequest
{
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; }
}