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
    public class RatingRepository : GenericRepositoryAsync<Rating>, IRatingRepository
    {
        public RatingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
