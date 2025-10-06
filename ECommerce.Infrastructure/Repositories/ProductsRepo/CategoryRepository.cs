using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.IRepositories.ProductsRepo;
using ECommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.ProductsRepo
{
    public class CategoryRepository : GenericRepositoryAsync<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
