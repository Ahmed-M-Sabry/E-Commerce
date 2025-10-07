using ECommerce.Application.Common;
using ECommerce.Application.IServices;
using ECommerce.Application.IServices.IProductServ;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.IRepositories.ProductsRepo;
using ECommerce.Infrastructure.Repositories.ProductsRepo;
using Microsoft.AspNetCore.Http;
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

        // ✅ Edit Product (بدون تعديل الصور)
        public async Task<Result<Product>> EditProductAsync(Product updatedProduct)
        {
            var existing = await _productRepository.GetByIdAsync(updatedProduct.Id, p => p.Photos);
            if (existing == null)
                return Result<Product>.Failure("Product not found.", ErrorType.NotFound);

            existing.Name = updatedProduct.Name;
            existing.Description = updatedProduct.Description;
            existing.NewPrice = updatedProduct.NewPrice;
            existing.OldPrice = updatedProduct.OldPrice;
            existing.CategoryId = updatedProduct.CategoryId;

            await _productRepository.UpdateAsync(existing);
            return Result<Product>.Success(existing);
        }

        // ✅ Delete Product + Photos
        public async Task<Result<bool>> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id, p => p.Photos);
            if (product == null)
                return Result<bool>.Failure("Product not found.", ErrorType.NotFound);

            // ✅ حذف الصور من السيرفر
            foreach (var photo in product.Photos)
            {
                _fileService.DeleteFile(photo.ImageName);
            }

            await _productRepository.DeleteAsync(product);
            return Result<bool>.Success(true);
        }

        // ✅ Get Product By Id
        public async Task<Result<Product>> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id, p => p.Photos, p => p.Category);
            if (product == null)
                return Result<Product>.Failure("Product not found.", ErrorType.NotFound);

            return Result<Product>.Success(product);
        }

        // ✅ Get All Products
        public async Task<Result<List<Product>>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync(p => p.Photos, p => p.Category);
            return Result<List<Product>>.Success(products.ToList());
        }
    }
}
