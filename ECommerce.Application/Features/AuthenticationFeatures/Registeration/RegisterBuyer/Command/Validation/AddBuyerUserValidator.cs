using ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterBuyer.Command.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterBuyer.Command.Validation
{
    public class AddBuyerUserValidator : AbstractValidator<AddBuyerUserCommand>
    {
        public AddBuyerUserValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is Requird.")
                .MinimumLength(4).WithMessage("Full Name must be at least 2 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
        }
    }
}
