using BWRecipeBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BWRecipeBook.Data
{
    public class ApplicationDbContext : IdentityDbContext<RBUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Recipe> Recipes { get; set; } = default!;
        public virtual DbSet<RecipeBook> RecipeBooks { get; set; } = default!;
        public virtual DbSet<Ingredient> Ingredient { get; set; } = default!;
    }
}