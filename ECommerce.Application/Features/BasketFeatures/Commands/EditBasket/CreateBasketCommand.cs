using ECommerce.Application.Common;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Commands.EditBasket
{
    public class CreateBasketCommand : IRequest<Result<CustomerBasket>>
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }
    }

}
