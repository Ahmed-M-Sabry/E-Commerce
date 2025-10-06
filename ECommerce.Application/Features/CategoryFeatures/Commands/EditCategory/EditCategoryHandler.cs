using ECommerce.Application.Common;
using ECommerce.Application.IServices.ProductServ;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CategoryFeatures.Commands.EditCategory
{
    public class EditCategoryHandler : IRequestHandler<EditCategoryCommand, Result<Category>>
    {
        private readonly ICategoryService _categoryService;

        public EditCategoryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Result<Category>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description
            };

            return await _categoryService.UpdateAsync(category);
        }
    }
}
