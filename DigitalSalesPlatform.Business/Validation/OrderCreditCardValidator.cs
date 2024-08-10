using DigitalSalesPlatform.Schema;
using FluentValidation;

namespace DigitalSalesPlatform.Business.Validation;

public class OrderCreditCardValidator : AbstractValidator<OrderCreditCardRequest>
{
    public OrderCreditCardValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Lütfen kullanıcı bilgisini giriniz.");
        RuleFor(x => x.CVC)
            .NotEmpty().WithMessage("Lütfen CVC bilgisini giriniz.")
            .MaximumLength(3).WithMessage("CVC 3 haneli olması gerekiyor.");
        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Lütfen kart bilgisini giriniz.")
            .MaximumLength(16).MinimumLength(16).WithMessage("Lütfen kart bilgilerinizi doğru giriniz.");

    }
}