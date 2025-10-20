using ECommerce.API.ApplicationBase;
using ECommerce.Application.Features.BasketFeatures.Commands.DeleteBasket;
using ECommerce.Application.Features.BasketFeatures.Commands.EditBasket;
using ECommerce.Application.Features.BasketFeatures.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ApplicationControllerBase
    {

        [HttpGet("Get-Basket")]
        public async Task<IActionResult> GetBasket([FromQuery]string id)
        {
            var result = await Mediator.Send(new GetBasketQuery { Id = id });
            return result.ResultStatusCode();
        }

        [HttpPost("Create-Basket")]
        public async Task<IActionResult> CreateBasket([FromBody] CreateBasketCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpDelete("Delete-Basket")]
        public async Task<IActionResult> DeleteBasket([FromQuery]string id)
        {
            var result = await Mediator.Send(new DeleteBasketCommand { Id = id });
            return result.ResultStatusCode();
        }
    }

}
