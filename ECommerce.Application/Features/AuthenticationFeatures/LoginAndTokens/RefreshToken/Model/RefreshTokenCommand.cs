using ECommerce.Application.Common;
using ECommerce.Domain.AuthenticationHepler;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.LoginAndTokens.RefreshToken.Model
{
    public class RefreshTokenCommand : IRequest<Result<ResponseAuthModel>>
    {

    }
}
