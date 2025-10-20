using ECommerce.Application.Common;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.IServices.IBasket
{
    public interface ICustomerBasketService
    {
        Task<Result<CustomerBasket>> GetBasketAsync(string basketId);
        Task<Result<CustomerBasket>> UpdateBasketAsync(CustomerBasket basket);
        Task<Result<bool>> DeleteBasketAsync(string basketId);
    }

}
