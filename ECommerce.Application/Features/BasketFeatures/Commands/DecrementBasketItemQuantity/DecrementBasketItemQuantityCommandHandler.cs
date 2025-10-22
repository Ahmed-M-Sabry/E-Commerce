using ECommerce.Application.Common;
using ECommerce.Application.IServices.IBasket;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Commands.DecrementItemQuantity
{
    public class DecrementBasketItemQuantityCommandHandler : IRequestHandler<DecrementBasketItemQuantityCommand, Result<bool>>
    {
        private readonly ICustomerBasketService _basketService;

        public DecrementBasketItemQuantityCommandHandler(ICustomerBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<Result<bool>> Handle(DecrementBasketItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var basketResult = await _basketService.GetBasketAsync(request.BasketId);
            if (!basketResult.IsSuccess || basketResult.Value == null)
                return Result<bool>.Failure("Basket not found.", ErrorType.NotFound);

            var basket = basketResult.Value;
            var item = basket.basketItems.FirstOrDefault(i => i.Id == request.ItemId);
            if (item == null)
                return Result<bool>.Failure("Item not found in basket.", ErrorType.NotFound);

            item.Quantity -= 1;
            if (item.Quantity <= 0)
            {
                basket.basketItems.Remove(item); 
            }

            var updated = await _basketService.UpdateBasketAsync(basket);
            if (!updated.IsSuccess)
                return Result<bool>.Failure("Failed to decrement item quantity.", ErrorType.InternalServerError);

            return Result<bool>.Success(true, "Item quantity decremented successfully.");
        }
    }
}
