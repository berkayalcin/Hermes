using FluentValidation;
using Hermes.API.User.Domain.Requests;

namespace Hermes.API.User.Domain.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(m => m.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();
            RuleFor(m => m.OldPassword)
                .NotNull()
                .NotEmpty();

            RuleFor(m => m.NewPassword)
                .NotNull()
                .NotEmpty()
                .MinimumLength(6)
                .Must((dto, s) => !s.Equals(dto.OldPassword))
                .WithMessage("Old password cannot be same with new password.");
        }
    }
}