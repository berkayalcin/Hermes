using FluentValidation;
using Hermes.API.Media.Domain.Requests;

namespace Hermes.API.Media.Domain.Validators
{
    public class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
    {
        public UploadImageRequestValidator()
        {
            RuleFor(r => r.Name).NotNull().NotEmpty();
            RuleFor(r => r.Base64String).NotNull().NotEmpty()
                .Matches("^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$");
        }
    }
}