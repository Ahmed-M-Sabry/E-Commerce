using ECommerce.Application.Common;
using ECommerce.Application.IServices;
using ECommerce.Application.IServices.IBasket;
using ECommerce.Application.IServices.IProductServ;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Commands.DecrementItemQuantity
{
    public class DecrementBasketItemQuantityCommandHandler : IRequestHandler<DecrementBasketItemQuantityCommand, Result<bool>>
    {
        private readonly ICustomerBasketService _basketService;
        private readonly IProductService _productService;

        public DecrementBasketItemQuantityCommandHandler(ICustomerBasketService basketService, IProductService productService)
        {
            _basketService = basketService;
            _productService = productService;
        }

        public async Task<Result<bool>> Handle(DecrementBasketItemQuantityCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.BasketId))
                return Result<bool>.Failure("Basket ID is required.", ErrorType.BadRequest);

            if (request.ItemId <= 0)
                return Result<bool>.Failure("Invalid item ID.", ErrorType.BadRequest);

            var productResult = await _productService.GetProductByIdAsync(request.ItemId);
            if (!productResult.IsSuccess)
                return Result<bool>.Failure("Product not found.", ErrorType.NotFound);

            var basketResult = await _basketService.GetBasketAsync(request.BasketId);
            if (!basketResult.IsSuccess || basketResult.Value == null)
                return Result<bool>.Failure("Basket not found.", ErrorType.NotFound);

            var basket = basketResult.Value;
            var item = basket.basketItems.FirstOrDefault(i => i.Id == request.ItemId);
            if (item == null)
                return Result<bool>.Failure("Item not found in basket.", ErrorType.NotFound);

            item.quantity -= 1;
            if (item.quantity <= 0)
            {
                basket.basketItems.Remove(item); 
            }
            else
            {
                var product = productResult.Value;
                item.name = product.Name;
                item.price = product.NewPrice;
                item.description = product.Description;
                item.category = product.Category?.Name ?? "Unknown";
                item.image = product.Photos?.FirstOrDefault()?.ImageName ?? "/Uploads/Defaults/defaultphoto.png";
            }

            var updated = await _basketService.UpdateBasketAsync(basket);
            if (!updated.IsSuccess)
                return Result<bool>.Failure("Failed to decrement item quantity.", ErrorType.InternalServerError);

            return Result<bool>.Success(true, "Item quantity decremented successfully.");
        }
    }
}