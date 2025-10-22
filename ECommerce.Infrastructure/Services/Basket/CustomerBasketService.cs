using ECommerce.Application.Common;
using ECommerce.Application.IServices.IBasket;
using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepositories.IBasketRpo;
using ECommerce.Infrastructure.Repositories.BasketRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services.Basket
{
    public class CustomerBasketService : ICustomerBasketService
    {
            private readonly ICustomerBasketRepository _basketRepository;

            public CustomerBasketService(ICustomerBasketRepository basketRepository)
            {
                _basketRepository = basketRepository;
            }

            public async Task<Result<CustomerBasket>> GetBasketAsync(string basketId)
            {
                if (string.IsNullOrWhiteSpace(basketId))
                    return Result<CustomerBasket>.Failure("Basket ID is required.", ErrorType.BadRequest);

                var basket = await _basketRepository.GetBasketAsync(basketId);
                if (basket == null)
                    return Result<CustomerBasket>.Failure("Basket not found.", ErrorType.NotFound);

                return Result<CustomerBasket>.Success(basket);
            }

            public async Task<Result<CustomerBasket>> UpdateBasketAsync(CustomerBasket basket)
            {
                if (basket == null || string.IsNullOrWhiteSpace(basket.Id))
                    return Result<CustomerBasket>.Failure("Invalid basket data.", ErrorType.BadRequest);

                foreach (var item in basket.basketItems)
                {
                    if (item.quantity <= 0)
                        return Result<CustomerBasket>.Failure($"Item {item.Id} has invalid quantity.", ErrorType.BadRequest);
                }

                var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);
                if (updatedBasket == null)
                    return Result<CustomerBasket>.Failure("Failed to update basket.", ErrorType.InternalServerError);

                return Result<CustomerBasket>.Success(updatedBasket, "Basket updated successfully.");
            }

            public async Task<Result<bool>> DeleteBasketAsync(string basketId)
            {
                if (string.IsNullOrWhiteSpace(basketId))
                    return Result<bool>.Failure("Basket ID is required.", ErrorType.BadRequest);

                var deleted = await _basketRepository.DeleteBasketAsync(basketId);
                if (!deleted)
                    return Result<bool>.Failure("Failed to delete basket or basket not found.", ErrorType.NotFound);

                return Result<bool>.Success(true, "Basket deleted successfully.");
            }
        }

    }
