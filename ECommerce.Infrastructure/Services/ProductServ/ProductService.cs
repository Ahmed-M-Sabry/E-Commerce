using ECommerce.Application.Comman;
using ECommerce.Application.Comman.Enum;
using ECommerce.Application.Comman.ProductParam;
using ECommerce.Application.Common;
using ECommerce.Application.IServices;
using ECommerce.Application.IServices.IProductServ;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.IRepositories.ProductsRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

            product.Photos = uploadResult.Value.Select(path => new ProductPhoto { ImageName = path }).ToList();

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
            var product = await _productRepository.GetByIdAsync(id, p => p.Photos, p => p.Category , s=>s.Seller,s=>s.Ratings);
            if (product == null)
                return Result<Product>.Failure("Product not found.", ErrorType.NotFound);

            EnsureDefaultPhoto(product);

            return Result<Product>.Success(product);
        }

        public async Task<Result<List<Product>>> GetAllProductsAsync()
        {
            var products = await _productRepository
                .GetAllAsync(p => p.Photos, p => p.Category)
                .ToListAsync();

            return Result<List<Product>>.Success(products);
        }

        
        public async Task<Result<KeysetPagination<Product>>> GetAllProductsKeysetPaginationAsync(int? lastId , int pageSize = 20)
        {
            var query = _productRepository.GetAllAsync(p => p.Photos, p => p.Category);

            if (lastId.HasValue)
                query = query.Where(p => p.Id > lastId.Value);

            var items = await query.Take(pageSize + 1).ToListAsync();

            bool hasMore = items.Count > pageSize;
            var data = items.Take(pageSize).ToList();
            var nextLastId = hasMore ? data.Last().Id : (int?)null;

            var result = new KeysetPagination<Product>
            {
                Data = items,
                HasMore = hasMore,
                LastIndex = nextLastId
            };

            return Result<KeysetPagination<Product>>.Success(result);
        }

        public async Task<Result<PagedResult<Product>>> GetAllProductsPaginationAsync(ProductParams productParams)
        {
            var query = _productRepository.GetAllAsync(p => p.Photos, p => p.Category ,s=>s.Ratings);

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

            foreach (var product in products)
            {
                EnsureDefaultPhoto(product);
            }
            var result = new PagedResult<Product>
            {
                Data = products,
                TotalCount = totalCount,
                PageNumber = productParams.PageNumber,
                PageSize = productParams.PageSize
            };

            return Result<PagedResult<Product>>.Success(result);
        }

        //-----------------------------------------------------
        public async Task<Result<KeysetPagination<Product>>> GetAllProductsKeysetPaginationAsync(
            string? cursor = null,                 
            int pageSize = 20,
            string? search = null,
            int? categoryId = null,
            string sortBy = "Id",                   
            bool isDescending = false)              
        {
            var query = _productRepository.GetAllAsync(p => p.Photos, p => p.Category);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var pattern = $"%{search}%";
                query = query.Where(p =>
                    EF.Functions.Like(p.Name, pattern) ||
                    EF.Functions.Like(p.Description, pattern));

                if(query.ToListAsync() == null )
                    return Result<KeysetPagination<Product>>.Failure("Not found any Product With This Name",ErrorType.NotFound);
            }

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            query = sortBy.ToLower() switch
            {
                "price" => isDescending
                    ? query.OrderByDescending(p => p.NewPrice)
                    : query.OrderBy(p => p.NewPrice),

                "date" => isDescending
                    ? query.OrderByDescending(p => p.CreatedDate)
                    : query.OrderBy(p => p.CreatedDate),

                "name" => isDescending
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),

                _ => isDescending
                    ? query.OrderByDescending(p => p.Id)
                    : query.OrderBy(p => p.Id)
            };

            if (!string.IsNullOrEmpty(cursor))
            {
                switch (sortBy.ToLower())
                {
                    case "price":
                        if (decimal.TryParse(cursor, out var priceCursor))
                            query = isDescending
                                ? query.Where(p => p.NewPrice < priceCursor)
                                : query.Where(p => p.NewPrice > priceCursor);
                        break;

                    case "date":
                        if (DateTime.TryParse(cursor, out var dateCursor))
                            query = isDescending
                                ? query.Where(p => p.CreatedDate < dateCursor)
                                : query.Where(p => p.CreatedDate > dateCursor);
                        break;

                    case "name":
                        query = isDescending
                            ? query.Where(p => string.Compare(p.Name, cursor) < 0)
                            : query.Where(p => string.Compare(p.Name, cursor) > 0);
                        break;

                    default:
                        if (int.TryParse(cursor, out var idCursor))
                            query = isDescending
                                ? query.Where(p => p.Id < idCursor)
                                : query.Where(p => p.Id > idCursor);
                        break;
                }
            }

            var items = await query.Take(pageSize + 1).ToListAsync();

            bool hasMore = items.Count > pageSize;
            var data = items.Take(pageSize).ToList();

            string? nextCursor = null;
            if (hasMore)
            {
                var lastItem = data.Last();

                nextCursor = sortBy.ToLower() switch
                {
                    "price" => lastItem.NewPrice.ToString(),
                    "date" => lastItem.CreatedDate.ToString("O"),
                    "name" => lastItem.Name,
                    _ => lastItem.Id.ToString()
                };
            }
            foreach (var product in data)
            {
                EnsureDefaultPhoto(product);
            }
            var result = new KeysetPagination<Product>
            {
                Data = data,
                HasMore = hasMore,
                LastCursor = nextCursor
            };

            return Result<KeysetPagination<Product>>.Success(result);
        }
        private void EnsureDefaultPhoto(Product product)
        {
            if (product.Photos == null || !product.Photos.Any())
            {
                product.Photos = new List<ProductPhoto>
        {
            new ProductPhoto
            {
                Id = 0,
                ProductId = product.Id,
                ImageName = "/Uploads/Defaults/defaultphoto.png"
            }
        };
            }
        }


    }
}
