using ECommerce.Application.Common;
using ECommerce.Application.IServices;
using ECommerce.Application.IServices.IProductServ;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.IRepositories.ProductsRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services.ProductServ
{
    public class ProductPhotoService : IProductPhotoService
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        private readonly IIdentityServies _identityService; 

        public ProductPhotoService(
            IPhotoRepository photoRepository,
            IProductRepository productRepository,
            IFileService fileService,
            IIdentityServies identityService)
        {
            _photoRepository = photoRepository;
            _productRepository = productRepository;
            _fileService = fileService;
            _identityService = identityService;
        }

        public async Task<Result<List<ProductPhoto>>> AddPhotosAsync(int productId, IFormFileCollection photos)
        {
            if (photos == null || photos.Count == 0)
                return Result<List<ProductPhoto>>.Failure("No photos provided.", ErrorType.BadRequest);

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return Result<List<ProductPhoto>>.Failure("Product not found.", ErrorType.NotFound);

            var userId = _identityService.GetUserId();
            if (userId == null || product.SellerId != userId)
                return Result<List<ProductPhoto>>.Failure("You are not authorized to modify this product.", ErrorType.Forbidden);

            var uploadResult = await _fileService.UploadMultipleFilesAsync(photos, "Products", "image");
            if (!uploadResult.IsSuccess)
                return Result<List<ProductPhoto>>.Failure(uploadResult.Message, ErrorType.BadRequest);

            var newPhotos = uploadResult.Value.Select(path => new ProductPhoto
            {
                ProductId = productId,
                ImageName = path
            }).ToList();

            foreach (var photo in newPhotos)
            {
                await _photoRepository.AddAsync(photo);
            }

            return Result<List<ProductPhoto>>.Success(newPhotos, "Photos uploaded successfully.");
        }

        public async Task<Result<bool>> DeletePhotoAsync(int photoId)
        {
            var photo = await _photoRepository.GetByIdAsync(photoId);
            if (photo == null)
                return Result<bool>.Failure("Photo not found.", ErrorType.NotFound);

            var product = await _productRepository.GetByIdAsync(photo.ProductId);
            var userId = _identityService.GetUserId();

            if (userId == null || product == null || product.SellerId != userId)
                return Result<bool>.Failure("You are not authorized to delete this photo.", ErrorType.Forbidden);

            _fileService.DeleteFile(photo.ImageName);
            await _photoRepository.DeleteAsync(photo);

            return Result<bool>.Success(true, "Photo deleted successfully.");
        }

        public async Task<Result<bool>> DeleteAllPhotosAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
                     
            if (product == null)
                return Result<bool>.Failure("Product not found.", ErrorType.NotFound);

            var userId = _identityService.GetUserId();
            if (userId == null || product.SellerId != userId)
                return Result<bool>.Failure("You are not authorized to delete photos for this product.", ErrorType.Forbidden);

            var photos = await _photoRepository.GetAllByProductIdAsync(productId);
            if (!photos.Any())
                return Result<bool>.Failure("No photos found for this product.", ErrorType.NotFound);

            foreach (var photo in photos)
            {
                _fileService.DeleteFile(photo.ImageName);
                await _photoRepository.DeleteAsync(photo);
            }

            return Result<bool>.Success(true, "All product photos deleted successfully.");
        }

        public async Task<Result<IEnumerable<ProductPhoto>>> GetPhotosByProductIdAsync(int productId)
        {
            var photos = await _photoRepository.GetAllByProductIdAsync(productId);
            if (!photos.Any())
                return Result<IEnumerable<ProductPhoto>>.Failure("No photos found for this product.", ErrorType.NotFound);

            return Result<IEnumerable<ProductPhoto>>.Success(photos);
        }
    }
}
