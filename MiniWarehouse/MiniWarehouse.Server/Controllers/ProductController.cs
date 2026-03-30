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
        public async Task<ActionResult<Product>> CreateProduct(ProductCreate product)
        {
            if (ModelState.IsValid)
            {
                await productService.AddAsync(product);

                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductCreate product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!productService.Exists(id))
                return NotFound();

            var updated = await productService.UpdateAsync(id, product);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            if (!productService.Exists(id))
                return NotFound();

            var deleted = await productService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("search", Name = "SearchProducts")]
        public async Task<ActionResult<List<Product>>> SearchAllProducts([FromQuery] ProductSearch productSearch)
        {
            var products = await productService.SearchProductsAsync(productSearch);
            return Ok(products);
        }
    }
}
