using ECommerce.Application.Common;
using ECommerce.Application.IServices.ProductServ;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CategoryFeatures.Queries.GetCategoryById
{

    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<Category>>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoryByIdHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Result<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetByIdAsync(request.Id);
        }
    }
}
