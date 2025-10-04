using ECommerce.Application.Common;
using ECommerce.Application.Features.AuthenticationFeatures.Password.RestPassword.Command;
using ECommerce.Application.IServices;
using ECommerce.Domain.AuthenticationHepler;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.Password.RestPassword.Handler
{
    public class SendTokenToRestPasswordHandler : IRequestHandler<SendTokenToRestPasswordCommand, Result<string>>
    {
        private readonly IIdentityServies _identityServies;
        private readonly IEmailService _emailService;

        public SendTokenToRestPasswordHandler(IIdentityServies identityServies, IEmailService emailService)
        {
            _identityServies = identityServies;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(SendTokenToRestPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityServies.GetUserByEmailAsync(request.Email);

            if (user == null)
                return Result<string>.Failure("Can't Send Rest Password Token to this email", ErrorType.BadRequest);

            var restPasswordToken = await _identityServies.GetRestPasswordTokenAsync(user);

            var restPasswordLink = $"{CommonLinks.SendEmailTo}/Rest-Password?userId={user.Id}&token={restPasswordToken}";

            await _emailService.SendResetPasswordEmailAsync(user.Email, "Rest Password Link", restPasswordLink);

            return Result<string>.Success("Rest Password Link send Successflly ");
        }
    }
}
