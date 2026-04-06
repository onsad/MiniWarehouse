using Microsoft.AspNetCore.Mvc;
using MiniWarehouse.Models;
using MiniWarehouse.Services;

namespace MiniWarehouse.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet(Name = "GetAllProducts")]
        public ActionResult<List<Product>> GetProducts()
        {
            var products = productService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id:guid}", Name = "GetProductById")]
        public ActionResult<Product> GetProduct(Guid id)
        {
            var product = productService.GetById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct(ProductCreate product)
        {
            var result = productService.Add(product);

            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetProduct),
                new { id = result.Data!.Id },
                result.Data);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Product> UpdateProduct(Guid id, ProductCreate product)
        {
            var result = productService.Update(id, product);

            if (!result.Success)
            {
                return result.Error switch
                {
                    "NotFound" => NotFound(),
                    "CategoryNotFound" => BadRequest("Category not found."),
                    _ => StatusCode(500)
                };
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteProduct(Guid id)
        {
            var result = productService.Delete(id);

            if (!result.Success)
            {
                return result.Error switch
                {
                    "NotFound" => NotFound(),
                    _ => StatusCode(500)
                };
            }

            return NoContent();
        }

        [HttpGet("search", Name = "SearchProducts")]
        public async Task<ActionResult<List<Product>>> SearchAllProducts([FromQuery] ProductSearch productSearch)
        {
            var products = productService.SearchProducts(productSearch);
            return Ok(products);
        }
    }
}
