using ECommerce.API.ApplicationBase;
using ECommerce.Application.Features.ProductFeatures.Commands.CreateProduct;
using ECommerce.Application.Features.ProductFeatures.Commands.DeleteProduct;
using ECommerce.Application.Features.ProductFeatures.Commands.EditProduct;
using ECommerce.Application.Features.ProductFeatures.Queries.GetAllProductByPagination;
using ECommerce.Application.Features.ProductFeatures.Queries.GetProductById;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ApplicationControllerBase
    {

        // Test KeysetPagination
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Test KeysetPagination
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts(long? lastId = null, int pageSize = 20)
        {
            var query = _context.Products.OrderBy(p => p.Id);

            if (lastId.HasValue)
                query = (IOrderedQueryable<Domain.Entities.Products.Product>)query.Where(p => p.Id > lastId.Value);

            var items = await query.Take(pageSize + 1).ToListAsync();

            bool hasMore = items.Count > pageSize;
            var data = items.Take(pageSize).ToList();
            var nextLastId = hasMore ? data.Last().Id : (long?)null;

            return Ok(new
            {
                data,
                nextLastId,
                hasMore
            });
        }


        [HttpGet("Get-All-Products")]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductByPaginationQuery query)
        {
            var result = await Mediator.Send(query);
            return result.ResultStatusCode();
        }

        [HttpGet("Get-Product-By-Id")]
        public async Task<IActionResult> GetProductById([FromQuery] GetProductByIdQuery query)
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

        [HttpPut("Edit-Product")]
        public async Task<IActionResult> EditProduct([FromForm] EditProductCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpDelete("Delete-Product")]
        public async Task<IActionResult> DeleteProduct([FromQuery] DeleteProductCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }
    }
}
