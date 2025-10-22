using ECommerce.Application.Common;
using MediatR;
using System;

namespace ECommerce.Application.Features.BasketFeatures.Commands.IncrementItemQuantity
{
    public class IncrementBasketItemQuantityCommand : IRequest<Result<bool>>
    {
        public string BasketId { get; set; } = string.Empty;
        public int ItemId { get; set; }
    }
}