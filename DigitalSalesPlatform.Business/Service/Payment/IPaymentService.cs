using DigitalSalesPlatform.Schema.Payment;

namespace DigitalSalesPlatform.Business.Service;

public interface IPaymentService
{
    Task<PaymentResponse> ProcessPayment(PaymentRequest request);
}