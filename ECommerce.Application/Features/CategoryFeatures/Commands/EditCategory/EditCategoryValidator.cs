using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CategoryFeatures.Commands.EditCategory
{
    public class EditCategoryValidator : AbstractValidator<EditCategoryCommand>
    {
        public EditCategoryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid category ID.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}
