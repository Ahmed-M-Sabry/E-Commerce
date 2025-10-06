using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.IRepositories.ProductsRepo
{
    public interface ICategoryRepository : IGenericRepositoryAsync<Category>
    {
        Task<string> GetCategoryNameByIdAsync(int id);
        Task<Category> GetCategoryByNameAsync(string name);
        Task<Category> DeleteCategory(int id);
        Task<Category> RestoreCategory(int id);

        Task<IReadOnlyList<Category>> GetAllCategoriesForUserAsync();
        Task<IReadOnlyList<Category>> GetAllCategoriesForAdminAsync();
    }
}
