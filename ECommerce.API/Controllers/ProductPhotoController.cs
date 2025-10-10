using ECommerce.API.ApplicationBase;
using ECommerce.Application.Features.ProductPhotoFeatures.Commands.AddProductPhotos;
using ECommerce.Application.Features.ProductPhotoFeatures.Commands.DeleteAllPhotos;
using ECommerce.Application.Features.ProductPhotoFeatures.Commands.DeletePhoto;
using ECommerce.Application.Features.ProductPhotoFeatures.Queries.GetPhotosByProductId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPhotoController : ApplicationControllerBase
    {
        [HttpPost("Add-Photos")]
        public async Task<IActionResult> AddPhotos([FromForm] AddProductPhotosCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpDelete("Delete-Photo")]
        public async Task<IActionResult> DeletePhoto([FromQuery] DeletePhotoCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpDelete("Delete-All-Photos")]
        public async Task<IActionResult> DeleteAllPhotos([FromQuery] DeleteAllPhotosCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpGet("Get-Photos-By-ProductId"),AllowAnonymous]
        public async Task<IActionResult> GetPhotos([FromQuery] GetPhotosByProductIdQuery query)
        {
            var result = await Mediator.Send(query);
            return result.ResultStatusCode();
        }
    }
}
