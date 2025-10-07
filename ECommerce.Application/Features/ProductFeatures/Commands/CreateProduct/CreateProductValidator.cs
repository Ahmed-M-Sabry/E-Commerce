using FluentValidation;

namespace ECommerce.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.NewPrice)
                .GreaterThan(0).WithMessage("New price must be greater than zero.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID is required.");

            RuleFor(x => x.PhotosFiles)
                .NotNull().WithMessage("At least one product image is required.")
                .Must(p => p.Count > 0).WithMessage("You must upload at least one photo.");
        }
    }
}
