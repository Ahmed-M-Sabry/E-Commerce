using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepositories.IBasketRpo;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.BasketRepo
{
    public class CustomerBasketRepository : ICustomerBasketRepository
    {
        private readonly IDatabase _database;
        public CustomerBasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public Task<bool> DeleteBasketAsync(string id)
        {
            return _database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string id)
        {
            var result = await _database.StringGetAsync(id);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<CustomerBasket>(result);
            }
            return null;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            if (basket == null || string.IsNullOrWhiteSpace(basket.Id))
                return null;

            var existingBasket = await GetBasketAsync(basket.Id);
            if (existingBasket != null)
            {
                existingBasket.basketItems = basket.basketItems;
                basket = existingBasket;
            }

            var result = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(3));
            if (result)
                return await GetBasketAsync(basket.Id);

            return null;
        }
        //public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        //{
        //    var existingBasket = await GetBasketAsync(basket.Id);
        //    if (existingBasket != null)
        //    {
        //        foreach (var newItem in basket.basketItems)
        //        {
        //            var oldItem = existingBasket.basketItems.FirstOrDefault(x => x.Id == newItem.Id);
        //            if (oldItem != null)
        //            {
        //                //oldItem.Quantity += newItem.Quantity;
        //                oldItem.Quantity = newItem.Quantity;
        //            }
        //            else
        //            {
        //                existingBasket.basketItems.Add(newItem);
        //            }
        //        }
        //        basket.basketItems = existingBasket.basketItems;
        //    }

        //    var result = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(3));
        //    if (result)
        //        return await GetBasketAsync(basket.Id);

        //    return null;
        //}

    }
}
    //public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    //{
    //    var _basket = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(3));
    //    if (_basket)
    //    {
    //        return await GetBasketAsync(basket.Id);
    //    }
    //    return null;
    //}

    //public async Task<CustomerBasket> RemoveItemAsync(string basketId, int itemId)
    //    {
    //        var basket = await GetBasketAsync(basketId);
    //        if (basket == null) return null;

    //        basket.basketItems = basket.basketItems.Where(i => i.Id != itemId).ToList();

    //        var result = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(3));
    //        return result ? basket : null;
    //    }
