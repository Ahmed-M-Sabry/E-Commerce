using ECommerce.Application.Comman;
using ECommerce.Application.IServices.ProductServ;
using ECommerce.Domain.IRepositories.ProductsRepo;
using ECommerce.Infrastructure.Services.ProductServ;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerce.API.Controllers
{
    [Route("errors/{statusCode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpPatch, HttpHead, HttpOptions]
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult(new ErrorResponse(statusCode));
        }
    }
}
