using FluentValidation;
using Hermes.API.Advertisement.Domain.Requests;

namespace Hermes.API.Advertisement.Domain.Validators
{
    public class CreateAdvertisementRequestValidator : AbstractValidator<CreateAdvertisementRequest>
    {
        public CreateAdvertisementRequestValidator()
        {
            RuleFor(r => r.Title).NotEmpty().NotNull();
            RuleFor(r => r.Description).NotEmpty().NotNull().MinimumLength(50).MaximumLength(1000);
            RuleFor(r => r.UserId).NotEmpty().NotNull();
            RuleFor(r => r.CategoryId).NotEmpty().NotNull();
        }
    }
}