using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform;

public class CouponResponse : BaseResponse
{
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; }
}