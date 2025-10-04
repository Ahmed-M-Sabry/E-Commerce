using ECommerce.Application.Common;
using ECommerce.Domain.AuthenticationHepler;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.LoginAndTokens.LoginUser.Command.Model
{
    public class UserLogInCommand : IRequest<Result<ResponseAuthModel>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
