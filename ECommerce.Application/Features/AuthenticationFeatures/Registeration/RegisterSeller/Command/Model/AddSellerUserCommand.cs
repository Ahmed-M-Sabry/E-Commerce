using ECommerce.Application.Common;
using ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterSeller.Command.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterSeller.Command.Model
{
    public class AddSellerUserCommand : IRequest<Result<RegisterSellerDto>>
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? StoreName { get; set; }     
        public string? StoreAddress { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
