using DigitalSalesPlatform.Schema;
using FluentValidation;

namespace DigitalSalesPlatform.Business.Validation;

public class ProductValidator : AbstractValidator<ProductRequest>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Lütfen ürün adını giriniz.");
        RuleFor(x => x.Stock).NotEmpty().WithMessage("Lütfen stok bilgisi giriniz.");
        RuleFor(x => x.PointsPercentage).NotEmpty().WithMessage("Lütfen puan bilgisi giriniz.");
        RuleFor(x => x.MaxPoints).NotEmpty().WithMessage("Lütfen maksimum puan bilgisi giriniz.");
        RuleFor(x => x.CategoryIds).NotEmpty().WithMessage("Lütfen kategori bilgisi giriniz.");
        RuleFor(x => x.Price).NotEmpty().WithMessage("Lütfen ürünün fiyat bilgisi giriniz.");
        RuleFor(x => x.IsActive).NotEmpty().WithMessage("Lütfen ürünün aktif/pasif bilgisi giriniz.");
    }
}