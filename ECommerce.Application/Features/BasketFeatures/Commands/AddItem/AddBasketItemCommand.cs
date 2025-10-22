using ECommerce.Application.Common;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Commands.AddItem
{
    public class AddBasketItemCommand : IRequest<Result<CustomerBasket>>
    {
        public string BasketId { get; set; } = string.Empty;
        public BasketItem Item { get; set; }
    }
}

