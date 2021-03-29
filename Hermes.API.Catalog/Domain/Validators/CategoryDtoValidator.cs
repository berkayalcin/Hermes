using FluentValidation;
using Hermes.API.Catalog.Domain.Models;

namespace Hermes.API.Catalog.Domain.Validators
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(p => p.Name).NotNull().NotEmpty().MinimumLength(3);
            RuleFor(p => p.ImageUrl).NotNull().NotEmpty().MinimumLength(30);
            RuleFor(p => p.Description).NotNull().NotEmpty().MinimumLength(30);
            RuleFor(p => p.Slug).Null();
        }
    }
}