using ECommerce.Application.Common;
using ECommerce.Application.IServices.IBasket;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Commands.RemoveItem
{
    public class RemoveBasketItemCommandHandler : IRequestHandler<RemoveBasketItemCommand, Result<bool>>
    {
        private readonly ICustomerBasketService _basketService;

        public RemoveBasketItemCommandHandler(ICustomerBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<Result<bool>> Handle(RemoveBasketItemCommand request, CancellationToken cancellationToken)
        {
            var basketResult = await _basketService.GetBasketAsync(request.BasketId);
            if (!basketResult.IsSuccess || basketResult.Value == null)
                return Result<bool>.Failure("Basket not found.", ErrorType.NotFound);

            var basket = basketResult.Value;
            var item = basket.basketItems.FirstOrDefault(i => i.Id == request.ItemId);
            if (item == null)
                return Result<bool>.Failure("Item not found in basket.", ErrorType.NotFound);

            basket.basketItems.Remove(item);
            var updated = await _basketService.UpdateBasketAsync(basket);

            if (!updated.IsSuccess)
                return Result<bool>.Failure("Failed to remove item.", ErrorType.InternalServerError);

            return Result<bool>.Success(true, "Item removed successfully.");
        }
    }
}
