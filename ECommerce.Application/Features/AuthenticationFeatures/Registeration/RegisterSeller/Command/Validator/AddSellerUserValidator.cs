using ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterSeller.Command.Model;
using FluentValidation;

namespace ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterSeller.Command.Validator
{
    public class AddSellerUserValidator : AbstractValidator<AddSellerUserCommand>
    {
        public AddSellerUserValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MinimumLength(4).WithMessage("Full name must be at least 4 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MinimumLength(10).WithMessage("Address must be at least 10 characters.")
                .MaximumLength(100).WithMessage("Address must not exceed 100 characters.");

            RuleFor(x => x.StoreName)
                .NotEmpty().WithMessage("Store name is required.")
                .MinimumLength(10).WithMessage("Store name must be at least 10 characters.")
                .MaximumLength(100).WithMessage("Store name must not exceed 100 characters.");

            RuleFor(x => x.StoreAddress)
                .NotEmpty().WithMessage("Store address is required.")
                .MinimumLength(10).WithMessage("Store address must be at least 10 characters.")
                .MaximumLength(100).WithMessage("Store address must not exceed 100 characters.");
        }
    }
}
