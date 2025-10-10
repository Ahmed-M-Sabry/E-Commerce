using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.IRepositories.ProductsRepo;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.ProductsRepo
{
    public class PhotoRepository : GenericRepositoryAsync<ProductPhoto>, IPhotoRepository
    {
        public PhotoRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<IEnumerable<ProductPhoto>> GetAllByProductIdAsync(int productId)
        {
            return await _dbContext.Set<ProductPhoto>()
                .AsNoTracking()
                .Where(p => p.ProductId == productId)
                .ToListAsync();
        }
    }
}
