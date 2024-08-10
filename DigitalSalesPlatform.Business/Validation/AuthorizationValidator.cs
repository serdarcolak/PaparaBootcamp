using DigitalSalesPlatform.Schema;
using FluentValidation;

namespace DigitalSalesPlatform.Business.Validation;

public class AuthorizationValidator : AbstractValidator<AuthorizationRequest>
{
    public AuthorizationValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Lütfen email adresinizi giriniz.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Lütfen şifrenizi giriniz.");
    }
}