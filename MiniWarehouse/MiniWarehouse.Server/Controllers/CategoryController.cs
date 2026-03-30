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
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                await categoryService.AddCategoryAsync(category);

                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ensure the incoming model has the requested id
            category.Id = id;

            if (!categoryService.Exists(id))
                return NotFound();

            var updated = await categoryService.UpdateCategoryAsync(id, category);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            if (!categoryService.Exists(id))
                return NotFound();

            var deleted = await categoryService.DeleteCategoryAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}