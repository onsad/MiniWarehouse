using Microsoft.AspNetCore.Mvc;
using MiniWarehouse.Models;
using MiniWarehouse.Services;

namespace MiniWarehouse.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(ProductService productService) : ControllerBase
    {
        [HttpGet(Name = "GetAllProducts")]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id:guid}", Name = "GetProductById")]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await productService.AddAsync(product);

                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }

            return BadRequest(ModelState);
        }
    }
}
