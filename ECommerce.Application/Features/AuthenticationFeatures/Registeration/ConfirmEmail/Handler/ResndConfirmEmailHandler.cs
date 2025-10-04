using ECommerce.Application.Common;
using ECommerce.Application.Features.AuthenticationFeatures.Registeration.ConfirmEmail.Command;
using ECommerce.Application.IServices;
using ECommerce.Domain.AuthenticationHepler;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.Registeration.ConfirmEmail.Handler
{
    public class ResndConfirmEmailHandler : IRequestHandler<ResendConfirmEmailCommand, Result<string>>
    {
        private readonly IIdentityServies _identityServies;
        private readonly IEmailService _emailService;

        public ResndConfirmEmailHandler(IIdentityServies identityServies , IEmailService emailService)
        {
            _identityServies = identityServies;
            _emailService = emailService;
        }
        public async Task<Result<string>> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
        {

            var user = await _identityServies.GetUserByEmailAsync(request.Email);

            if(user == null)
                return Result<string>.Failure("Can't Send Confirmation to this email" , ErrorType.BadRequest);

            if (user.EmailConfirmed)
                return Result<string>.Failure("Email is confirmed.", ErrorType.BadRequest);

            var confirmToken = await _identityServies.GetEmailConfirmationTokenAsync(user);

            var confirmationLink = $"{CommonLinks.SendEmailTo}/confirm-email?userId={user.Id}&token={confirmToken}";

            await _emailService.SendEmailAsync(user.Email, "Resend Confirm email", confirmationLink);

            return Result<string>.Success("Confimation Link send Successflly ");
        }
    }
}
