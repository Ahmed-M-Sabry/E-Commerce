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
    public class CategoryRepository : GenericRepositoryAsync<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string> GetCategoryNameByIdAsync(int id)
        {
            return await _context.Categories
                .Where(c => c.Id == id && !c.IsDeleted)
                .Select(c => c.Name)
                .FirstOrDefaultAsync() ?? "Not Found";
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<IReadOnlyList<Category>> GetAllCategoriesForUserAsync()
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Category>> GetAllCategoriesForAdminAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category> DeleteCategory(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null)
                return null;

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Category> RestoreCategory(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null)
                return null;

            entity.IsDeleted = false;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
