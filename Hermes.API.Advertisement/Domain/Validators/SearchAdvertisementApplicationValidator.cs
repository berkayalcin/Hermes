using FluentValidation;
using Hermes.API.Advertisement.Domain.Models;

namespace Hermes.API.Advertisement.Domain.Validators
{
    public class AdvertisementApplicationValidator : AbstractValidator<AdvertisementApplicationDto>
    {
        public AdvertisementApplicationValidator()
        {
            RuleFor(r => r.AdvertisementId).NotNull().NotEmpty();
            RuleFor(r => r.ApplicantId).NotNull().NotEmpty();
            RuleFor(r => r.EstimatedBorrowDays).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}