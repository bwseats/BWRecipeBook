using System.ComponentModel.DataAnnotations;

namespace BWRecipeBook.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ingredient Name")]
        public string? Name { get; set; }

        [Required]
        public string? Measure { get; set; }

        // nav props
        public virtual ICollection<Recipe> Recipes { get; set; } = new HashSet<Recipe>();
    }
}
