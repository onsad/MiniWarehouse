using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MiniWarehouse.Models
{
    public class Category
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }
    }
}
