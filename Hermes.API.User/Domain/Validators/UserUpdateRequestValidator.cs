using FluentValidation;
using Hermes.API.User.Domain.Requests;

namespace Hermes.API.User.Domain.Validators
{
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(u => u.Firstname)
                .NotEmpty();
            RuleFor(u => u.Lastname)
                .NotEmpty();
            RuleFor(u => u.PhoneNumber)
                .NotEmpty()
                .Matches("^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s\\./0-9]*$");
        }
    }
}