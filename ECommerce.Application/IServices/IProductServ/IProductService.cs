using ECommerce.Application.Comman;
using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.IServices.IProductServ
{
    public interface IProductService
    {
        Task<Result<Product>> CreateProductAsync(Product product, IFormFileCollection photos);
        Task<Result<Product>> EditProductAsync(int Id, Product updatedProduct);
        Task<Result<bool>> DeleteProductAsync(int id);
        Task<Result<Product>> GetProductByIdAsync(int id);
        Task<Result<List<Product>>> GetAllProductsAsync();
        Task<Result<PagedResult<Product>>> GetAllProductsPaginationAsync(ProductParams productParams);
    }
}
