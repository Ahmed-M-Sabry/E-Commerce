using ECommerce.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.Registeration.ConfirmEmail.Command
{
    public class ConfirmEmailCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
