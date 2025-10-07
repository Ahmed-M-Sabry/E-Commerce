using ECommerce.API.ApplicationBase;
using ECommerce.Application.Features.ProductFeatures.Commands.CreateProduct;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ApplicationControllerBase
    {
        [HttpPost("Create-Product")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateProductCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

    }
}
