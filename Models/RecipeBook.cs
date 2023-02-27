using System.ComponentModel.DataAnnotations;

namespace BWRecipeBook.Models
{
    public class RecipeBook
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Recipe Book Title")]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }


        // navigation properties
        [Required]
        public string? RBUserId { get; set; }
        public virtual RBUser? RBUser { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; } = new HashSet<Recipe>();
    }
}
