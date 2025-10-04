using ECommerce.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.Password.RestPassword.Command
{
    public class SendTokenToRestPasswordCommand : IRequest<Result<string>>
    {
        public string Email { get; set; }
    }
}
