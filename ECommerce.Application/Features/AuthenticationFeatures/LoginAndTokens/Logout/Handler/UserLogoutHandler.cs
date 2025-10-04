using ECommerce.Application.Common;
using ECommerce.Application.Features.AuthenticationFeatures.LoginAndTokens.Logout.Command;
using ECommerce.Application.IServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.LoginAndTokens.Logout.Handler
{
    public class UserLogoutHandler : IRequestHandler<UserLogoutCommand, Result<bool>>
    {
        private readonly IIdentityServies _identityServies;
        public UserLogoutHandler(IIdentityServies identityServies)
        {
            _identityServies = identityServies;
        }
        public async Task<Result<bool>> Handle(UserLogoutCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityServies.RevokeRefreshTokenFromCookiesAsync();
            if(!result)
                return Result<bool>.Failure("Logout failed", ErrorType.InternalServerError);

            return Result<bool>.Success(true, "Logour Success");
        }
    }
}
