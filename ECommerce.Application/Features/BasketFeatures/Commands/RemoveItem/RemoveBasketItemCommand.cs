using ECommerce.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Commands.RemoveItem
{
    public class RemoveBasketItemCommand : IRequest<Result<bool>>
    {
        public string BasketId { get; set; } = string.Empty;
        public int ItemId { get; set; }
    }
}
