using ECommerce.Application.Common;
using MediatR;
using System;

namespace ECommerce.Application.Features.BasketFeatures.Commands.DecrementItemQuantity
{
    public class DecrementBasketItemQuantityCommand : IRequest<Result<bool>>
    {
        public string BasketId { get; set; } = string.Empty;
        public int ItemId { get; set; }
    }
}