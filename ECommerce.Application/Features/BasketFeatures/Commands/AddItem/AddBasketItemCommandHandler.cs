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

namespace ECommerce.Application.Features.BasketFeatures.Commands.AddItem
{
    public class AddBasketItemCommandHandler : IRequestHandler<AddBasketItemCommand, Result<CustomerBasket>>
    {
        private readonly ICustomerBasketService _basketService;
        private readonly IProductService _productService;

        public AddBasketItemCommandHandler(ICustomerBasketService basketService, IProductService productService)
        {
            _basketService = basketService;
            _productService = productService;
        }

        public async Task<Result<CustomerBasket>> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.BasketId))
                return Result<CustomerBasket>.Failure("Basket ID is required.", ErrorType.BadRequest);

            if (request.ItemId <= 0)
                return Result<CustomerBasket>.Failure("Invalid item ID.", ErrorType.BadRequest);

            var productResult = await _productService.GetProductByIdAsync(request.ItemId);
            if (!productResult.IsSuccess)
                return Result<CustomerBasket>.Failure(productResult.Message, productResult.ErrorType);

            var product = productResult.Value;

            if (product.StockQuantity <= 0)
                return Result<CustomerBasket>.Failure("Product is out of stock.", ErrorType.BadRequest);

            var basketResult = await _basketService.GetBasketAsync(request.BasketId);
            CustomerBasket basket;

            if (!basketResult.IsSuccess || basketResult.Value == null)
            {
                basket = new CustomerBasket
                {
                    Id = request.BasketId,
                    basketItems = new List<BasketItem>
                    {
                        new BasketItem
                        {
                            Id = product.Id,
                            quantity = 1,
                            name = product.Name,
                            price = product.NewPrice,
                            description = product.Description,
                            category = product.Category?.Name ?? "Unknown",
                            image = product.Photos?.FirstOrDefault()?.ImageName ?? "/Uploads/Defaults/defaultphoto.png"
                        }
                    }
                };
            }
            else
            {
                basket = basketResult.Value;
                var existingItem = basket.basketItems.FirstOrDefault(i => i.Id == request.ItemId);
                if (existingItem != null)
                {
                    if (existingItem.quantity + 1 > product.StockQuantity)
                        return Result<CustomerBasket>.Failure($"Cannot add more items. Only {product.StockQuantity} available in stock.", ErrorType.BadRequest);

                    existingItem.quantity += 1;
                    existingItem.name = product.Name;
                    existingItem.price = product.NewPrice;
                    existingItem.description = product.Description;
                    existingItem.category = product.Category?.Name ?? "Unknown";
                    existingItem.image = product.Photos?.FirstOrDefault()?.ImageName ?? "/Uploads/Defaults/defaultphoto.png";
                }
                else
                {
                    basket.basketItems.Add(new BasketItem
                    {
                        Id = product.Id,
                        quantity = 1,
                        name = product.Name,
                        price = product.NewPrice,
                        description = product.Description,
                        category = product.Category?.Name ?? "Unknown",
                        image = product.Photos?.FirstOrDefault()?.ImageName ?? "/Uploads/Defaults/defaultphoto.png"
                    });
                }
            }

            var updated = await _basketService.UpdateBasketAsync(basket);
            if (!updated.IsSuccess)
                return Result<CustomerBasket>.Failure("Failed to add item.", ErrorType.InternalServerError);

            return Result<CustomerBasket>.Success(updated.Value, "Item added successfully.");
        }
    }
}