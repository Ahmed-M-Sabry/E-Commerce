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
    public class PhotoRepository : GenericRepositoryAsync<Photo>, IPhotoRepository
    {
        public PhotoRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
