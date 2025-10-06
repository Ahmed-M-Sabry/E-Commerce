using ECommerce.Application.Common;
using ECommerce.Application.IServices.ProductServ;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.IRepositories.ProductsRepo;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services.ProductServ
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<Category>> CreateAsync(Category category)
        {
            if (category is null)
                return Result<Category>.Failure("Category cannot be null.", ErrorType.BadRequest);

            var existing = await _categoryRepository.GetCategoryByNameAsync(category.Name);
            if (existing is not null)
                return Result<Category>.Failure("Category name already exists.", ErrorType.Conflict);

            var created = await _categoryRepository.AddAsync(category);
            return Result<Category>.Success(created, "Category created successfully.");
        }

        public async Task<Result<Category>> UpdateAsync(Category category)
        {
            if (category is null)
                return Result<Category>.Failure("Category cannot be null.", ErrorType.BadRequest);

            var existing = await _categoryRepository.GetByIdAsync(category.Id);
            if (existing is null)
                return Result<Category>.Failure("Category not found.", ErrorType.NotFound);

            var nameConflict = await _categoryRepository.GetCategoryByNameAsync(category.Name);
            if (nameConflict is not null && nameConflict.Id != category.Id)
                return Result<Category>.Failure("Category name already exists.", ErrorType.Conflict);

            existing.Name = category.Name;
            existing.Description = category.Description;

            await _categoryRepository.UpdateAsync(existing);
            return Result<Category>.Success(existing, "Category updated successfully.");
        }

        public async Task<Result<Category>> DeleteAsync(int id)
        {
            var existing = await _categoryRepository.GetByIdAsync(id);
            if (existing is null)
                return Result<Category>.Failure("Category not found.", ErrorType.NotFound);

            if (existing.IsDeleted)
                return Result<Category>.Failure("Category already deleted.", ErrorType.BadRequest);

            var deleted = await _categoryRepository.DeleteCategory(id);
            return Result<Category>.Success(deleted, "Category deleted successfully.");
        }

        public async Task<Result<Category>> RestoreAsync(int id)
        {
            var existing = await _categoryRepository.GetByIdAsync(id);
            if (existing is null)
                return Result<Category>.Failure("Category not found.", ErrorType.NotFound);

            if (!existing.IsDeleted)
                return Result<Category>.Failure("Category is not deleted.", ErrorType.BadRequest);

            var restored = await _categoryRepository.RestoreCategory(id);
            return Result<Category>.Success(restored, "Category restored successfully.");
        }

        public async Task<Result<Category>> GetByIdAsync(int id)
        {
            var data = await _categoryRepository.GetByIdAsync(id);
            if (data is null)
                return Result<Category>.Failure("Category not found.", ErrorType.NotFound);

            return Result<Category>.Success(data);
        }

        public async Task<Result<string>> GetCategoryNameByIdAsync(int id)
        {
            var name = await _categoryRepository.GetCategoryNameByIdAsync(id);
            if (string.IsNullOrWhiteSpace(name) || name == "Not Found")
                return Result<string>.Failure("Category not found.", ErrorType.NotFound);

            return Result<string>.Success(name);
        }

        public async Task<Result<Category>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Category>.Failure("Name cannot be empty.", ErrorType.BadRequest);

            var data = await _categoryRepository.GetCategoryByNameAsync(name);
            if (data is null)
                return Result<Category>.Failure("Category not found.", ErrorType.NotFound);

            return Result<Category>.Success(data);
        }

        public async Task<Result<IEnumerable<Category>>> GetAllForUserAsync()
        {
            var data = await _categoryRepository.GetAllCategoriesForUserAsync();
            if (data == null || !data.Any())
                return Result<IEnumerable<Category>>.Failure("No categories available.", ErrorType.NotFound);

            return Result<IEnumerable<Category>>.Success(data);
        }

        public async Task<Result<IEnumerable<Category>>> GetAllForAdminAsync()
        {
            var data = await _categoryRepository.GetAllCategoriesForAdminAsync();
            if (data == null || !data.Any())
                return Result<IEnumerable<Category>>.Failure("No categories found.", ErrorType.NotFound);

            return Result<IEnumerable<Category>>.Success(data);
        }
    }
}
