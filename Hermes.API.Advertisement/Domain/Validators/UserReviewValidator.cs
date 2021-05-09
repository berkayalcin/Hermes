using FluentValidation;
using Hermes.API.Advertisement.Domain.Models;

namespace Hermes.API.Advertisement.Domain.Validators
{
    public class UserReviewValidator : AbstractValidator<UserReviewDto>
    {
        public UserReviewValidator()
        {
            RuleFor(r => r.Review).NotNull().NotEmpty();
            RuleFor(r => r.ApplicationId).NotNull().NotEmpty();
            RuleFor(r => r.ReviewedUserId).NotNull().NotEmpty();
            RuleFor(r => r.ReviewOwnerId).NotNull().NotEmpty();
            RuleFor(r => r.Stars).NotNull().NotEmpty();
        }
    }
}