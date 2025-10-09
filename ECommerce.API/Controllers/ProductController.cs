using ECommerce.API.ApplicationBase;
using ECommerce.Application.Features.ProductFeatures.Commands.CreateProduct;
using ECommerce.Application.Features.ProductFeatures.Commands.DeleteProduct;
using ECommerce.Application.Features.ProductFeatures.Commands.EditProduct;
using ECommerce.Application.Features.ProductFeatures.Commands.Queries.GetAllProductByPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ApplicationControllerBase
    {
        [HttpGet("Get-All-Products")]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductByPaginationQuery query)
        {
            var result = await Mediator.Send(query);
            return result.ResultStatusCode();
        }

        [HttpPost("Create-Product")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }
        [HttpPut("Edit-Product"),AllowAnonymous]
        public async Task<IActionResult> EditProduct([FromForm] EditProductCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }
        [HttpDelete("Delete-Product")]
        public async Task<IActionResult> DeleteProduct([FromForm] DeleteProductCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }
    }
}
