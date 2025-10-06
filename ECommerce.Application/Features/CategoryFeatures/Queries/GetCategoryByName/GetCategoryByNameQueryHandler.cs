using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.IServices.ProductServ;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CategoryFeatures.Queries.GetCategoryByName
{
    public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, Result<Category>>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoryByNameQueryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Result<Category>> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByNameAsync(request.Name);

            if (category == null)
                return Result<Category>.Failure("Category not found.",ErrorType.NotFound);

            return Result<Category>.Success(category.Value);
        }
    }
}
