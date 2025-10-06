using ECommerce.API.ApplicationBase;
using ECommerce.Application.Features.CategoryFeatures.Commands.CreateCategory;
using ECommerce.Application.Features.CategoryFeatures.Commands.DeleteCategory;
using ECommerce.Application.Features.CategoryFeatures.Commands.EditCategory;
using ECommerce.Application.Features.CategoryFeatures.Commands.RestoreCategory;
using ECommerce.Application.Features.CategoryFeatures.Queries.GetActiveCategories;
using ECommerce.Application.Features.CategoryFeatures.Queries.GetAllCategories;
using ECommerce.Application.Features.CategoryFeatures.Queries.GetCategoryById;
using ECommerce.Application.Features.CategoryFeatures.Queries.GetCategoryByName;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ApplicationControllerBase
    {
        [HttpPost("Create-Category")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpPut("Edit-Category")]
        public async Task<IActionResult> EditCategory([FromForm] EditCategoryCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpDelete("Soft-Delete-Category")]
        public async Task<IActionResult> SoftDeleteCategory([FromQuery] DeleteCategoryCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpPatch("Restore-Category")]
        public async Task<IActionResult> RestoreCategory([FromQuery] RestoreCategoryCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpGet("Get-All-Categories-Admin")]
        public async Task<IActionResult> GetAllCategoriesForAdmin()
        {
            var result = await Mediator.Send(new GetAllCategoriesForAdminQuery());
            return result.ResultStatusCode();
        }

        [HttpGet("Get-All-Categories-User")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategoriesForUser()
        {
            var result = await Mediator.Send(new GetAllCategoriesForUserQuery());
            return result.ResultStatusCode();
        }

        [HttpGet("Get-Category-ById")]
        public async Task<IActionResult> GetCategoryById([FromQuery] GetCategoryByIdQuery query)
        {
            var result = await Mediator.Send(query);
            return result.ResultStatusCode();
        }

        [HttpGet("Get-Category-ByName")]
        public async Task<IActionResult> GetCategoryByName([FromQuery] GetCategoryByNameQuery query)
        {
            var result = await Mediator.Send(query);
            return result.ResultStatusCode();
        }
    }
}
