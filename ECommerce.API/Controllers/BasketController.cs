using ECommerce.API.ApplicationBase;
using ECommerce.Application.Features.BasketFeatures.Commands.AddItem;
using ECommerce.Application.Features.BasketFeatures.Commands.DecrementItemQuantity;
using ECommerce.Application.Features.BasketFeatures.Commands.DeleteBasket;
using ECommerce.Application.Features.BasketFeatures.Commands.EditBasket;
using ECommerce.Application.Features.BasketFeatures.Commands.IncrementItemQuantity;
using ECommerce.Application.Features.BasketFeatures.Commands.RemoveItem;
using ECommerce.Application.Features.BasketFeatures.Queries.GetBasketById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ApplicationControllerBase
    {
        [HttpGet("Get-Basket")]
        public async Task<IActionResult> GetBasket([FromQuery] string id)
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
        public async Task<IActionResult> DeleteBasket([FromQuery] string id)
        {
            var result = await Mediator.Send(new DeleteBasketCommand { Id = id });
            return result.ResultStatusCode();
        }

        [HttpPost("Add-Item")]
        public async Task<IActionResult> AddItem([FromBody] AddBasketItemCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpDelete("Remove-Item")]
        public async Task<IActionResult> RemoveItem([FromQuery] RemoveBasketItemCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpPut("Increment-Item-Quantity")]
        public async Task<IActionResult> IncrementItemQuantity([FromBody] IncrementBasketItemQuantityCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpPut("Decrement-Item-Quantity")]
        public async Task<IActionResult> DecrementItemQuantity([FromBody] DecrementBasketItemQuantityCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }
    }
}