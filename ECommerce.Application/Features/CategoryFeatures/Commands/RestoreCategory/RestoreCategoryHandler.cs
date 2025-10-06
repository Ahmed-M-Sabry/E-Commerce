using ECommerce.Application.Common;
using ECommerce.Application.IServices.ProductServ;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CategoryFeatures.Commands.RestoreCategory
{
    public class RestoreCategoryHandler : IRequestHandler<RestoreCategoryCommand, Result<Category>>
    {
        private readonly ICategoryService _categoryService;

        public RestoreCategoryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Result<Category>> Handle(RestoreCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _categoryService.RestoreAsync(request.Id);
        }
    }
}
