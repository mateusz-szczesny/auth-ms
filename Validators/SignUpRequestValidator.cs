using Auth.Requests;
using FluentValidation;

namespace Auth.Validators
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .NotNull()
                .WithMessage("Username cannot be blank.")
                .MinimumLength(6)
                .MaximumLength(12)
                .WithMessage("Uername must be 6-12 characters long.");
            RuleFor(x => x.Email)
                .NotEmpty().NotNull().WithMessage("Email cannot be blank.")
                .EmailAddress().WithMessage("Email must be in email pattern.");

        }
    }
}