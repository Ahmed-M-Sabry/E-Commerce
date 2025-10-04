using ECommerce.Application.Common;
using ECommerce.Application.Features.AuthenticationFeatures.ConfirmEmail.Command;
using ECommerce.Application.IServices;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.ConfirmEmail.Handler
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Result<string>>
    {
        private readonly IIdentityServies _identityServies;

        public ConfirmEmailHandler(IIdentityServies identityServies)
        {
            _identityServies = identityServies;
        }

        public async Task<Result<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityServies.IsUserExist(request.UserId);
            if (user == null)
                return Result<string>.Failure("Invalid user ID.", ErrorType.NotFound);

            var tokenBytes = WebEncoders.Base64UrlDecode(request.Token);
            var decodedToken = Encoding.UTF8.GetString(tokenBytes);

            var result = await _identityServies.ConfirmEmailByTokenAsync(user, decodedToken);

            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                return Result<string>.Failure(errors, ErrorType.BadRequest);
            }

            return Result<string>.Success("Email confirmed successfully!");
        }
    }
}
