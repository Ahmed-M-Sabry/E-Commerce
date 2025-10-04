using AutoMapper;
using ECommerce.Application.Comman;
using ECommerce.Application.Common;
using ECommerce.Application.Features.AuthenticationFeatures.RegisterBuyer.Command.Dtos;
using ECommerce.Application.Features.AuthenticationFeatures.RegisterBuyer.Command.Model;
using ECommerce.Application.IServices;
using ECommerce.Domain.AuthenticationHepler;
using ECommerce.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.AuthenticationFeatures.RegisterBuyer.Command.Handler
{
    public class AddBuyerUserHandler : IRequestHandler<AddBuyerUserCommand, Result<RegisterUserDto>>
    {
        private readonly IIdentityServies _identityServies;
        private readonly IValidator<AddBuyerUserCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        public AddBuyerUserHandler(
            IValidator<AddBuyerUserCommand> validator,
            IMapper mapper,
            IIdentityServies identityServies,
            IEmailService emailService)
        {
            _validator = validator;
            _mapper = mapper;
            _identityServies = identityServies;
            _emailService = emailService;

        }

        public async Task<Result<RegisterUserDto>> Handle(AddBuyerUserCommand request, CancellationToken cancellationToken)
        {
            // Validation
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<RegisterUserDto>.Failure(string.Join(" | ", errors), ErrorType.BadRequest);
            }

            // Email Exists
            if (await _identityServies.IsEmailExist(request.Email, cancellationToken))
            {
                return Result<RegisterUserDto>.Failure("Email already registered.", ErrorType.Conflict);
            }

            // Map
            var newUser = _mapper.Map<ApplicationUser>(request);
            if (newUser == null)
            {
                return Result<RegisterUserDto>.Failure("User mapping failed.", ErrorType.InternalServerError);
            }

            // Create User
            var result = await _identityServies.CreateBuyerUserAsync(newUser, request.Password, cancellationToken);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result<RegisterUserDto>.Failure(string.Join(" | ", errors), ErrorType.UnprocessableEntity);
            }

            // Generate Token
            var token = await _identityServies.CreateJwtToken(newUser, cancellationToken);

            // Success
            var response = new RegisterUserDto
            {
                UserId = newUser.Id,
                Email = newUser.Email,
                Token = token,
                Role = ApplicationRoles.Buyer
            };
            // Send Confimation Token
            var confirmToken = await _identityServies.GetEmailConfirmationTokenAsync(newUser);

            var confirmationLink = $"{CommonLinks.SendEmailTo}/confirm-email?userId={newUser.Id}&token={confirmToken}";

            await _emailService.SendEmailAsync(newUser.Email, "Confirm your email", confirmationLink);

            return Result<RegisterUserDto>.Success(response);
        }
    }
}