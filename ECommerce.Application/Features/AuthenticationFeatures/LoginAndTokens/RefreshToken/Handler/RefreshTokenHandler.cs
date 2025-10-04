﻿using ECommerce.Application.Common;
using ECommerce.Application.Features.AuthenticationFeatures.LoginAndTokens.RefreshToken.Model;
using ECommerce.Application.IServices;
using ECommerce.Domain.AuthenticationHepler;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.LoginAndTokens.RefreshToken.Handler
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<ResponseAuthModel>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityServies _identityServies;
        public RefreshTokenHandler(
            IHttpContextAccessor httpContextAccessor ,
             IIdentityServies identityServies)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityServies = identityServies;
        }

        public async Task<Result<ResponseAuthModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Result<ResponseAuthModel>.Failure("No refresh token provided." , ErrorType.NotFound);

            var result = await _identityServies.RefreshTokenAsunc(refreshToken);

            if (!string.IsNullOrEmpty(result.Message))
                return Result<ResponseAuthModel>.Failure(result.Message, ErrorType.NotFound);

            return Result<ResponseAuthModel>.Success(result);


        }
    }
}
