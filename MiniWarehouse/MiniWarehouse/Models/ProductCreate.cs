using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MiniWarehouse.Services;

namespace MiniWarehouse.Models
{
    public class ProductCreate : IValidatableObject
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Check that category Guid is provided
            if (CategoryId == Guid.Empty)
            {
                yield return new ValidationResult("Category is required.", new[] { nameof(CategoryId) });
                yield break;
            }

            // Try to resolve CategoryService from DI container
            var categoryService = validationContext.GetRequiredService<CategoryService>();
            if (categoryService == null)
            {
                // If service not available, skip existence check (or optionally return an error)
                yield break;
            }

            if (!categoryService.Exists(CategoryId))
            {
                yield return new ValidationResult("Category does not exist.", new[] { nameof(CategoryId) });
            }
        }
    }
}
