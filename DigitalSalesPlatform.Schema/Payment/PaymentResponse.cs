using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema.Payment;

public class PaymentResponse : BaseResponse
{
    public bool IsSuccessful { get; set; }
    public string Message { get; set; }
}