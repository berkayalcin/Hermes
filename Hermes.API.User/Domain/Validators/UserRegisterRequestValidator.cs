using FluentValidation;
using Hermes.API.User.Domain.Requests;

namespace Hermes.API.User.Domain.Validators
{
    public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRegisterRequestValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(u => u.Firstname)
                .NotEmpty();
            RuleFor(u => u.Lastname)
                .NotEmpty();
            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(6);
            RuleFor(u => u.PhoneNumber)
                .NotEmpty()
                .Matches("^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s\\./0-9]*$");
            RuleFor(u => u.IsAcceptedEula)
                .NotNull()
                .Equal(true);
        }
    }
}