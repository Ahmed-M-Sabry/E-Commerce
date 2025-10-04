using ECommerce.Application.Comman;
using ECommerce.Application.Common;
using ECommerce.Application.Features.AuthenticationFeatures.RegisterBuyer.Command.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.RegisterBuyer.Command.Model
{
    public class AddBuyerUserCommand : IRequest<Result<RegisterUserDto>>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
