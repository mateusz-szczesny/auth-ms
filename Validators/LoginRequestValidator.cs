using Auth.Requests;
using FluentValidation;

namespace Auth.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty()
                .NotNull()
                .WithMessage("Identifier cannot be blank.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("password cannot be blank.");
        }
    }
}