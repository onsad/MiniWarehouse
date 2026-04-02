using Microsoft.AspNetCore.Http.HttpResults;
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

            var result = await productService.UpdateAsync(id, product);

            return result switch
            {
                ProductServiceResult.NotFound => NotFound(),
                ProductServiceResult.CategoryNotFound => BadRequest("Category not found."),
                _ => NoContent()
            };
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await productService.DeleteAsync(id);

            return result switch
            {
                ProductServiceResult.NotFound => NotFound(),
                _ => NoContent()
            };
        }

        [HttpGet("search", Name = "SearchProducts")]
        public async Task<ActionResult<List<Product>>> SearchAllProducts([FromQuery] ProductSearch productSearch)
        {
            var products = await productService.SearchProductsAsync(productSearch);
            return Ok(products);
        }
    }
}
