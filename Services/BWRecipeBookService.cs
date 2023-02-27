using BWRecipeBook.Data;
using BWRecipeBook.Models;
using BWRecipeBook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BWRecipeBook.Services
{
    public class BWRecipeBookService : IBWRecipeBookService
    {
        private readonly ApplicationDbContext _context;

        public BWRecipeBookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddRecipeToRecipeBookAsync(int recipeBookIds, int recipeId)
        {
            throw new NotImplementedException();
        }

        public Task AddRecipeToRecipeBooksAsync(IEnumerable<int> recipeBookIds, int recipeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RecipeBook>> GetRBUserRecipeBooksAsync(string rbUserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsRecipeInRecipeBook(int recipeBookId, int recipeId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllRecipeRecipeBooksAsync(int recipeId)
        {
            throw new NotImplementedException();
        }
    }
}
