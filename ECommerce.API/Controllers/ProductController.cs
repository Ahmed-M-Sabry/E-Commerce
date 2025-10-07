using ECommerce.API.ApplicationBase;
using ECommerce.Application.Features.ProductFeatures.Commands.CreateProduct;
using ECommerce.Application.Features.ProductFeatures.Commands.EditProduct;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ApplicationControllerBase
    {
        [HttpPost("Create-Product")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }
        [HttpPut("Edit-Product")]
        public async Task<IActionResult> EditProduct([FromForm] EditProductCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }
    }
}
