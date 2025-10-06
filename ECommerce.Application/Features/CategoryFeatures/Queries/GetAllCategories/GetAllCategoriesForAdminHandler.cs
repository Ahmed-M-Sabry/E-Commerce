using ECommerce.Application.Common;
using ECommerce.Application.IServices.ProductServ;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CategoryFeatures.Queries.GetAllCategories
{
    public class GetAllCategoriesForAdminHandler : IRequestHandler<GetAllCategoriesForAdminQuery, Result<IEnumerable<Category>>>
    {
        private readonly ICategoryService _categoryService;

        public GetAllCategoriesForAdminHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Result<IEnumerable<Category>>> Handle(GetAllCategoriesForAdminQuery request, CancellationToken cancellationToken)
        {
            var allCategories = await _categoryService.GetAllForAdminAsync();

            if (allCategories == null || allCategories.Value == null || !allCategories.Value.Any())
                return Result<IEnumerable<Category>>.Failure("No categories found", ErrorType.NotFound);


            return Result<IEnumerable<Category>>.Success(allCategories.Value, "Categories retrieved successfully");
        }
    }
}
