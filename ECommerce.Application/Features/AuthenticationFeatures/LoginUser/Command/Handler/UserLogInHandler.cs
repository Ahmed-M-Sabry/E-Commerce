using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Features.AuthenticationFeatures.LoginUser.Command.Model;
using ECommerce.Application.IServices;
using ECommerce.Domain.AuthenticationHepler;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.LoginUser.Command.Handler
{
    public class UserLogInHandler : IRequestHandler<UserLogInCommand, Result<ResponseAuthModel>>
    {
        private readonly IIdentityServies _identityServies;
        private readonly IValidator<UserLogInCommand> _validator;
        private readonly IMapper _mapper;

        public UserLogInHandler(
            IValidator<UserLogInCommand> validator,
            IMapper mapper,
            IIdentityServies identityServies)
        {
            _validator = validator;
            _mapper = mapper;
            _identityServies = identityServies;
        }
        public async Task<Result<ResponseAuthModel>> Handle(UserLogInCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<ResponseAuthModel>.Failure(string.Join(" | ", errors), ErrorType.BadRequest);
            }
            // if Email Not Exists
            var user = await _identityServies.GetUserByEmailAsync(request.Email);

            if (user is null)
                return Result<ResponseAuthModel>.Failure("Invalid credentials", ErrorType.Unauthorized);

            if (!user.EmailConfirmed)
            {
                return Result<ResponseAuthModel>.Failure("Please confirm your email before logging in.", ErrorType.Unauthorized);
            }

            // If Password incorrect
            var isPasswordValid = await _identityServies.IsPasswordExist(user, request.Password, cancellationToken);

            if (!isPasswordValid)
                return Result<ResponseAuthModel>.Failure("Invalid credentials", ErrorType.Unauthorized);

            // Generate Refresh Token 
            var response = await _identityServies.GenerateRefreshTokenAsync(user, request.RememberMe, cancellationToken);

            return Result<ResponseAuthModel>.Success(response);
        }
    }
}
