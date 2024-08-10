using DigitalSalesPlatform.Schema;
using FluentValidation;

namespace DigitalSalesPlatform.Business.Validation;

public class CouponValidator : AbstractValidator<CouponRequest>
{
    public CouponValidator()
    {
        RuleFor(x => x.Code).MaximumLength(10);
        RuleFor(x => x.Amount).NotEmpty().WithMessage("Lütfen kuponun miktarını giriniz.");
        RuleFor(x => x.ExpiryDate).NotEmpty().WithMessage("Lütfen kuponun bitiş tarihini giriniz.");
        RuleFor(x => x.IsActive).NotEmpty().WithMessage("Lütfen kuponun aktif/pasif bilgisini giriniz.");
    }
}