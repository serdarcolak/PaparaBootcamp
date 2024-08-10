using DigitalSalesPlatform.Schema;
using FluentValidation;

namespace DigitalSalesPlatform.Business.Validation;

public class OrderDetailValidator : AbstractValidator<OrderDetailRequest>
{
    public OrderDetailValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Lütfen sipariş vermek istediğiniz ürünü giriniz.");
        RuleFor(x => x.Quantity).NotEmpty().WithMessage("Lütfen sipariş vermek istediğiniz ürün miktarını giriniz.");

    }
}