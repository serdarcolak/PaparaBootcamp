using DigitalSalesPlatform.Schema.Payment;

namespace DigitalSalesPlatform.Business.Service;

public class PaymentService : IPaymentService
{
    public async Task<PaymentResponse> ProcessPayment(PaymentRequest request)
    {
        return await Task.FromResult(new PaymentResponse
        {
            IsSuccessful = true,
            Message = "Payment processed successfully"
        });
    }
}