using DigitalSalesPlatform.Schema;
using FluentValidation;

namespace DigitalSalesPlatform.Business.Validation;

public class UserValidator : AbstractValidator<UserRequest>
{
    public UserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Lütfen email adresinizi giriniz.")
            .EmailAddress().WithMessage("Lütfen geçerli bir mail adresi giriniz.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Lütfen parolanızı giriniz.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Lütfen adınızı giriniz.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Lütfen soyadınızı giriniz.");
    }
}