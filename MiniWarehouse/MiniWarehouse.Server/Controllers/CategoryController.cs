using Microsoft.AspNetCore.Mvc;
using MiniWarehouse.Models;
using MiniWarehouse.Services;

namespace MiniWarehouse.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Category>> GetCategories()
        {
            var categories = categoryService.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Category> GetCategory(Guid id)
        {
            var category = categoryService.GetById(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpGet("by-name/{name}")]
        public ActionResult<Category> GetCategoryByName(string name)
        {
            var category = categoryService.GetByName(name);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public ActionResult<Category> CreateCategory(Category category)
        {
            var result = categoryService.Add(category);

            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetCategory),
                new { id = result.Data!.Id },
                result.Data);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Category> UpdateCategory(Guid id, Category category)
        {
            var result = categoryService.Update(id, category);

            if (!result.Success)
            {
                return result.Error switch
                {
                    "NotFound" => NotFound(),
                    _ => StatusCode(500)
                };
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteCategory(Guid id)
        {
            var result = categoryService.Delete(id);

            if (!result.Success)
            {
                return result.Error switch
                {
                    "NotFound" => NotFound(),
                    "HasProducts" => BadRequest("Category is used by products."),
                    _ => StatusCode(500)
                };
            }

            return NoContent();
        }
    }
}