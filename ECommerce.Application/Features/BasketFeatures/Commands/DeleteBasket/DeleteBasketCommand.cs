using ECommerce.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Commands.DeleteBasket
{
    public class DeleteBasketCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; }
    }
}
