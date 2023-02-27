using BWRecipeBook.Models;

namespace BWRecipeBook.Services.Interfaces
{
    public interface IBWRecipeBookService
    {
        public Task AddRecipeToRecipeBookAsync(int recipeBookIds, int recipeId);

        public Task AddRecipeToRecipeBooksAsync(IEnumerable<int> recipeBookIds, int recipeId);

        public Task<IEnumerable<RecipeBook>> GetRBUserRecipeBooksAsync(string rbUserId);

        public Task<bool> IsRecipeInRecipeBook(int recipeBookId, int recipeId);

        public Task RemoveAllRecipeRecipeBooksAsync(int recipeId);
    }
}
