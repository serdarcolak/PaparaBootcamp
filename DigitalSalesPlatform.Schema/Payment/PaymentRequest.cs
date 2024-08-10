using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema.Payment;

public class PaymentRequest : BaseRequest
{
    public int UserId { get; set; }
    public string CardNumber { get; set; }
    public string ExpiryDate { get; set; }
    public string CVC { get; set; }
    public decimal Amount { get; set; }
}