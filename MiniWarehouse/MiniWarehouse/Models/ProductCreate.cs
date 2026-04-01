using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        }
    }
}
