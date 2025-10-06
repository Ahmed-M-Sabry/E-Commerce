using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.IServices.ProductServ
{
    public interface ICategoryService
    {
        Task<Result<Category>> GetByIdAsync(int id);
        Task<Result<string>> GetCategoryNameByIdAsync(int id);
        Task<Result<Category>> GetByNameAsync(string name);
        Task<Result<Category>> CreateAsync(Category category);
        Task<Result<Category>> UpdateAsync(Category category);
        Task<Result<Category>> DeleteAsync(int id);
        Task<Result<Category>> RestoreAsync(int id);
        Task<Result<IEnumerable<Category>>> GetAllForUserAsync();
        Task<Result<IEnumerable<Category>>> GetAllForAdminAsync();
    }
}
