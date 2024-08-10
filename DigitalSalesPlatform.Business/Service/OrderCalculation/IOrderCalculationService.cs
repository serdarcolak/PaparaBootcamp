using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;

namespace DigitalSalesPlatform.Business.Service;

public interface IOrderCalculationService
{
    Task CalculateOrderDigitalWalletDetailsAsync(Order order, OrderDigitalWalletRequest request);
    
    Task CalculateOrderCreditCardDetailsAsync(Order order, OrderCreditCardRequest request);
    Task ApplyCouponAsync(Order order, string couponCode);
    Task ApplyPointsAsync(Order order, User user);
    Task ApplyWalletBalanceAsync(Order order, User user);
    Task CalculatePointsEarnedAsync(Order order);
}