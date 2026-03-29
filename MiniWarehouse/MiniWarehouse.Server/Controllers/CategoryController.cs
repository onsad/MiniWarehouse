using Microsoft.AspNetCore.Mvc;
using MiniWarehouse.Models;
using MiniWarehouse.Services;

namespace MiniWarehouse.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(CategoryService categoryService) : ControllerBase
    {
        [HttpGet(Name = "GetAllCategories")]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            var categories = await categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:guid}", Name = "GetCategoryById")]
        public async Task<ActionResult<Category>> GetCategoryById(Guid id)
        {
            var category = await categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpGet("{name:alpha}", Name = "GetCategoryByName")]
        public async Task<ActionResult<Category>> GetCategoryByName(string name)
        {
            var category = await categoryService.GetCategoryByName(name);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Category category)
        {
            if (ModelState.IsValid)
            {
                await categoryService.AddCategoryAsync(category);

                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
            }

            return BadRequest(ModelState);
        }
    }
}