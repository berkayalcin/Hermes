using FluentValidation;
using Hermes.API.Advertisement.Domain.Models;

namespace Hermes.API.Advertisement.Domain.Validators
{
    public class FavoriteValidator : AbstractValidator<FavoriteDto>
    {
        public FavoriteValidator()
        {
            RuleFor(r => r.AdvertisementId).NotNull().NotEmpty();
            RuleFor(r => r.UserId).NotNull().NotEmpty();
        }
    }
}