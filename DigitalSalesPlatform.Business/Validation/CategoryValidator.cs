using DigitalSalesPlatform.Schema;
using FluentValidation;

namespace DigitalSalesPlatform.Business.Validation;

public class CategoryValidator : AbstractValidator<CategoryRequest>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Lütfen kategori adını giriniz.");
        RuleFor(x => x.Url).NotEmpty().WithMessage("Lütfen url bilgisini giriniz.");
        RuleFor(x => x.Tag).NotEmpty().WithMessage("Lütfen tag bilgisini giriniz.");
    }
}