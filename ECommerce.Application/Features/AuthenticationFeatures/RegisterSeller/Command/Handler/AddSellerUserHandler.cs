using AutoMapper;
using ECommerce.Application.Comman;
using ECommerce.Application.Common;
using ECommerce.Application.Features.AuthenticationFeatures.RegisterBuyer.Command.Dtos;
using ECommerce.Application.Features.AuthenticationFeatures.RegisterBuyer.Command.Model;
using ECommerce.Application.Features.AuthenticationFeatures.RegisterSeller.Command.Dos;
using ECommerce.Application.Features.AuthenticationFeatures.RegisterSeller.Command.Model;
using ECommerce.Application.IServices;
using ECommerce.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.RegisterSeller.Command.Handler
{
    public class AddSellerUserHandler : IRequestHandler<AddSellerUserCommand, Result<RegisterSellerDto>>
    {
        private readonly IIdentityServies _identityServies;
        private readonly IValidator<AddSellerUserCommand> _validator;
        private readonly IMapper _mapper;

        public AddSellerUserHandler(
            IValidator<AddSellerUserCommand> validator,
            IMapper mapper,
            IIdentityServies identityServies)
        {
            _validator = validator;
            _mapper = mapper;
            _identityServies = identityServies;
        }
        public async Task<Result<RegisterSellerDto>> Handle(AddSellerUserCommand request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();
                //var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
               return Result<RegisterSellerDto>.Failure(string.Join(" | ", errors), ErrorType.BadRequest);
            }

            // Email Exists
            if (await _identityServies.IsEmailExist(request.Email, cancellationToken))
            {
                return Result<RegisterSellerDto>.Failure("Email already registered.", ErrorType.Conflict);
            }

            // Map
            var newUser = _mapper.Map<ApplicationUser>(request);
            if (newUser == null)
            {
                return Result<RegisterSellerDto>.Failure("User mapping failed.", ErrorType.InternalServerError);
            }

            // Create User
            var result = await _identityServies.CreateSellerUserAsync(newUser, request.Password, cancellationToken);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result<RegisterSellerDto>.Failure(string.Join(" | ", errors), ErrorType.UnprocessableEntity);
            }

            // Generate Token
            var token = await _identityServies.CreateJwtToken(newUser, cancellationToken);

            // Success
            var response = new RegisterSellerDto
            {
                UserId = newUser.Id,
                Email = newUser.Email,
                Token = token,
                Role = ApplicationRoles.Seller
            };

            return Result<RegisterSellerDto>.Success(response);
        }
    
    }
}
