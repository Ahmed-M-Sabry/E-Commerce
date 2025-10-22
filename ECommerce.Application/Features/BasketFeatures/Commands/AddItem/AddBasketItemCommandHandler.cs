using ECommerce.Application.Common;
using ECommerce.Application.IServices.IBasket;
using ECommerce.Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Commands.AddItem
{
    public class AddBasketItemCommandHandler : IRequestHandler<AddBasketItemCommand, Result<CustomerBasket>>
    {
        private readonly ICustomerBasketService _basketService;

        public AddBasketItemCommandHandler(ICustomerBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<Result<CustomerBasket>> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Item == null || request.Item.Id <= 0)
                return Result<CustomerBasket>.Failure("Invalid item data.", ErrorType.BadRequest);

            if (request.Item.Quantity <= 0)
                request.Item.Quantity = 1;

            var basketResult = await _basketService.GetBasketAsync(request.BasketId);
            CustomerBasket basket;

            if (!basketResult.IsSuccess || basketResult.Value == null)
            {
                basket = new CustomerBasket
                {
                    Id = request.BasketId,
                    basketItems = new List<BasketItem> { request.Item }
                };
            }
            else
            {
                basket = basketResult.Value;
                var existingItem = basket.basketItems.FirstOrDefault(i => i.Id == request.Item.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity += request.Item.Quantity;
                }
                else
                {
                    basket.basketItems.Add(request.Item);
                }
            }

            var updated = await _basketService.UpdateBasketAsync(basket);
            if (!updated.IsSuccess)
                return Result<CustomerBasket>.Failure("Failed to add item.", ErrorType.InternalServerError);

            return Result<CustomerBasket>.Success(updated.Value, "Item added successfully.");
        }
    }
}