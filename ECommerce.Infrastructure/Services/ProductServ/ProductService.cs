using ECommerce.Application.Comman;
using ECommerce.Application.Comman.Enum;
using ECommerce.Application.Common;
using ECommerce.Application.IServices;
using ECommerce.Application.IServices.IProductServ;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.IRepositories.ProductsRepo;
using ECommerce.Infrastructure.Repositories.ProductsRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services.ProductServ
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository productRepository,
                              IPhotoRepository photoRepository,
                              IFileService fileService,
                              ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _photoRepository = photoRepository;
            _fileService = fileService;
        }

        public async Task<Result<Product>> CreateProductAsync(Product product, IFormFileCollection photos)
        {
            if (product == null)
                return Result<Product>.Failure("Product data is required.", ErrorType.BadRequest);

            var category = await _categoryRepository.GetByIdAsync(product.CategoryId);

            if (category == null)
                return Result<Product>.Failure("Category not found.", ErrorType.NotFound);

            var uploadResult = await _fileService.UploadMultipleFilesAsync(photos, "Products", "image");
            if (!uploadResult.IsSuccess)
                return Result<Product>.Failure(uploadResult.Message, ErrorType.BadRequest);

            product.Photos = uploadResult.Value.Select(path => new Photo { ImageName = path }).ToList();

            var added = await _productRepository.AddAsync(product);
            return Result<Product>.Success(added);
        }

        public async Task<Result<Product>> EditProductAsync(int Id , Product updatedProduct)
        {
            var existing = await _productRepository.GetByIdAsync(Id);
            if (existing == null)
                return Result<Product>.Failure("Product not found.", ErrorType.NotFound);

            var categoryExists = await _categoryRepository.GetByIdAsync(updatedProduct.CategoryId);
            if (categoryExists == null)
                return Result<Product>.Failure("Category not found.", ErrorType.NotFound);


            existing.Name = updatedProduct.Name;
            existing.Description = updatedProduct.Description;
            existing.NewPrice = updatedProduct.NewPrice;
            existing.OldPrice = updatedProduct.OldPrice;
            existing.CategoryId = updatedProduct.CategoryId;

            await _productRepository.UpdateAsync(existing);
            return Result<Product>.Success(existing);
        }

        public async Task<Result<bool>> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id, p => p.Photos);
            if (product == null)
                return Result<bool>.Failure("Product not found.", ErrorType.NotFound);

            foreach (var photo in product.Photos)
            {
                _fileService.DeleteFile(photo.ImageName);
            }

            await _productRepository.DeleteAsync(product);
            return Result<bool>.Success(true);
        }

        public async Task<Result<Product>> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id, p => p.Photos, p => p.Category);
            if (product == null)
                return Result<Product>.Failure("Product not found.", ErrorType.NotFound);

            return Result<Product>.Success(product);
        }

        public async Task<Result<List<Product>>> GetAllProductsAsync()
        {
            var products = _productRepository.GetAllAsync(p => p.Photos, p => p.Category).ToList();
            return Result<List<Product>>.Success(products);
        }

        public async Task<Result<PagedResult<Product>>> GetAllProductsPaginationAsync(ProductParams productParams)
        {
            var query = _productRepository.GetAllAsync(p => p.Photos, p => p.Category);

            if (!string.IsNullOrWhiteSpace(productParams.Search))
            {
                var search = $"%{productParams.Search}%";
                query = query.Where(p =>
                    EF.Functions.Like(p.Name, search) ||
                    EF.Functions.Like(p.Description, search));
            }

            if (productParams.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == productParams.CategoryId.Value);

            query = productParams.SortOption switch
            {
                ProductSortOption.PriceAsc => query.OrderBy(p => p.NewPrice),
                ProductSortOption.PriceDesc => query.OrderByDescending(p => p.NewPrice),
                ProductSortOption.NameDesc => query.OrderByDescending(p => p.Name),
                ProductSortOption.DateAddedDesc => query.OrderByDescending(p => p.CreatedDate),
                _ => query.OrderBy(p => p.Name)
            };

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((productParams.PageNumber - 1) * productParams.PageSize)
                .Take(productParams.PageSize)
                .ToListAsync();

            if (!products.Any())
                return Result<PagedResult<Product>>.Failure("No Product Found", ErrorType.NotFound);

            var result = new PagedResult<Product>
            {
                Data = products,
                TotalCount = totalCount,
                PageNumber = productParams.PageNumber,
                PageSize = productParams.PageSize
            };

            return Result<PagedResult<Product>>.Success(result);
        }

    }
}
