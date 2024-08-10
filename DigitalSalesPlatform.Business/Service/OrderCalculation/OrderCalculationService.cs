using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;

namespace DigitalSalesPlatform.Business.Service;

public class OrderCalculationService : IOrderCalculationService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderCalculationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CalculateOrderDigitalWalletDetailsAsync(Order order, OrderDigitalWalletRequest request)
    {
        foreach (var orderDetail in order.OrderDetails)
        {
            var product = await _unitOfWork.ProductRepository.GetById(orderDetail.ProductId);
            if (product != null && product.IsActive)
            {
                orderDetail.Price = product.Price * orderDetail.Quantity;
                orderDetail.InsertUser = Convert.ToString(request.UserId);
            }
        }

        order.TotalAmount = order.OrderDetails.Sum(od => od.Price);
    }

    public async Task CalculateOrderCreditCardDetailsAsync(Order order, OrderCreditCardRequest request)
    {
        foreach (var orderDetail in order.OrderDetails)
        {
            var product = await _unitOfWork.ProductRepository.GetById(orderDetail.ProductId);
            if (product != null && product.IsActive)
            {
                orderDetail.Price = product.Price * orderDetail.Quantity;
                orderDetail.InsertUser = Convert.ToString(request.UserId);
            }
        }

        order.TotalAmount = order.OrderDetails.Sum(od => od.Price);
    }

    public async Task ApplyCouponAsync(Order order, string couponCode)
    {
        var userCoupon = await _unitOfWork.CouponRepository.FirstOrDefault(c => c.Code == couponCode && c.IsActive);
        if (userCoupon != null && userCoupon.ExpiryDate >= DateTime.UtcNow)
        {
            if (userCoupon.Amount >= order.TotalAmount)
            {
                order.CouponAmount = order.TotalAmount;
                userCoupon.Amount -= order.TotalAmount;
                order.TotalAmount = 0;
            }
            else
            {
                order.CouponAmount = userCoupon.Amount;
                order.TotalAmount -= userCoupon.Amount;
                userCoupon.Amount = 0;
            }

            userCoupon.IsActive = userCoupon.Amount > 0;
            _unitOfWork.CouponRepository.Update(userCoupon);
        }
    }

    public async Task ApplyPointsAsync(Order order, User user)
    {
        if (user.PointsBalance > 0)
        {
            if (user.PointsBalance >= order.TotalAmount)
            {
                order.PointsUsed = order.TotalAmount;
                user.PointsBalance -= order.TotalAmount;
                order.TotalAmount = 0;
            }
            else
            {
                order.PointsUsed = user.PointsBalance;
                order.TotalAmount -= user.PointsBalance;
                user.PointsBalance = 0;
            }
        }
    }

    public async Task ApplyWalletBalanceAsync(Order order, User user)
    {
        var walletToUse = Math.Min(user.WalletBalance, order.TotalAmount);
        order.WalletUsed = walletToUse;
        user.WalletBalance -= walletToUse;
        order.TotalAmount -= walletToUse;
    }

    public async Task CalculatePointsEarnedAsync(Order order)
    {
        foreach (var orderDetail in order.OrderDetails)
        {
            var product = await _unitOfWork.ProductRepository.GetById(orderDetail.ProductId);
            if (product != null && product.IsActive)
            {
                var points = (orderDetail.Price * product.PointsPercentage / 100);
                if (points > product.MaxPoints)
                {
                    points = product.MaxPoints;
                }
                order.PointsEarned += points;
            }
        }
    }
}