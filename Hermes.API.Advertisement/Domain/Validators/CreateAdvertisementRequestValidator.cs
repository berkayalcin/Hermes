using FluentValidation;
using Hermes.API.Advertisement.Domain.Requests;

namespace Hermes.API.Advertisement.Domain.Validators
{
    public class CreateAdvertisementRequestValidator : AbstractValidator<CreateAdvertisementRequest>
    {
        public CreateAdvertisementRequestValidator()
        {
            RuleFor(r => r.Title).NotNull().NotEmpty();
            RuleFor(r => r.Description).NotEmpty().NotNull().MinimumLength(50).MaximumLength(1000);
            RuleFor(r => r.UserId).NotNull().NotEmpty();
            RuleFor(r => r.CategoryId).NotNull().NotEmpty();
            RuleFor(r => r.EstimatedBarrowDays).NotNull().NotEmpty();
            RuleFor(r => r.Longitude).NotNull().NotEmpty();
            RuleFor(r => r.Latitude).NotNull().NotEmpty();
            RuleFor(r => r.Images).NotNull().NotEmpty();
        }
    }

    public class UpdateAdvertisementRequestValidator : AbstractValidator<UpdateAdvertisementRequest>
    {
        public UpdateAdvertisementRequestValidator()
        {
            RuleFor(r => r.Id).NotNull().NotEmpty();
            RuleFor(r => r.Title).NotNull().NotEmpty();
            RuleFor(r => r.Description).NotEmpty().NotNull().MinimumLength(50).MaximumLength(1000);
            RuleFor(r => r.UserId).NotNull().NotEmpty();
            RuleFor(r => r.CategoryId).NotNull().NotEmpty();
            RuleFor(r => r.EstimatedBarrowDays).NotNull().NotEmpty();
            RuleFor(r => r.Longitude).NotNull().NotEmpty();
            RuleFor(r => r.Latitude).NotNull().NotEmpty();
            RuleFor(r => r.Images).NotNull().NotEmpty();
        }
    }
}