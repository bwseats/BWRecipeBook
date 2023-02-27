using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BWRecipeBook.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        public string? RBUserId { get; set; }
        
        [Required]
        [Display(Name = "Recipe Title")]
        public string? Title { get; set; }
        
        [Required]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        // nav props
        public virtual RBUser? RBUser { get; set; }
        public int RecipeBookId { get; set; }
        public virtual RecipeBook? RecipeBook { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; } = new HashSet<Ingredient>();
    }
}
