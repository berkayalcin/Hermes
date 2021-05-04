using FluentValidation;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Requests;

namespace Hermes.API.Advertisement.Domain.Validators
{
    public class SearchAdvertisementApplicationValidator : AbstractValidator<SearchAdvertisementApplicationRequest>
    {
        public SearchAdvertisementApplicationValidator()
        {
            RuleFor(r => r.AdvertisementId).NotNull().NotEmpty();
        }
    }

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