using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepositories.IOrderRepo;
using ECommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.OrderRepo
{
    public class CustomerBasketRepository : GenericRepositoryAsync<CustomerBasket> , ICustomerBasketRepository
    {
        public CustomerBasketRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
