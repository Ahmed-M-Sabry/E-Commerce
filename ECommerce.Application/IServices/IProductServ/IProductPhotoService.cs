using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.IServices.IProductServ
{
    public interface IProductPhotoService
    {
        Task<Result<List<ProductPhoto>>> AddPhotosAsync(int productId, IFormFileCollection photos);
        Task<Result<bool>> DeletePhotoAsync(int photoId);
        Task<Result<bool>> DeleteAllPhotosAsync(int productId);
        Task<Result<IEnumerable<ProductPhoto>>> GetPhotosByProductIdAsync(int productId);
    }
}
