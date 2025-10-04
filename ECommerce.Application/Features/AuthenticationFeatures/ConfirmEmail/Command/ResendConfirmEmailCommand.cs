using ECommerce.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.ConfirmEmail.Command
{
    public class ResendConfirmEmailCommand : IRequest<Result<string>>
    {
        public string Email { get; set; }
    }
}
