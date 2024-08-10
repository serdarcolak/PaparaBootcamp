using DigitalSalesPlatform.Schema;
using FluentValidation;

namespace DigitalSalesPlatform.Business.Validation;

public class OrderDigitalWalletValidator : AbstractValidator<OrderDigitalWalletRequest>
{
    public OrderDigitalWalletValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Lütfen kullanıcı bilgisini giriniz.");
        RuleFor(x => x.CouponCode).NotEmpty().WithMessage("Lütfen kupon kodunu bilgisini giriniz.");
        

    }
}