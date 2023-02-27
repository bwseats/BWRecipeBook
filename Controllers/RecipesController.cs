using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BWRecipeBook.Data;
using BWRecipeBook.Models;
using Microsoft.AspNetCore.Authorization;
using BWRecipeBook.Services.Interfaces;
using Mono.TextTemplating;

namespace BWRecipeBook.Controllers
{
    [Authorize]
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<RBUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IBWRecipeBookService _recipeBookService;

        public RecipesController(ApplicationDbContext context,
                                 UserManager<RBUser> userManager,
                                 IImageService imageService,
                                 IBWRecipeBookService recipeBookService)
        {
            _context = context;
            _userManager = userManager;
            _imageService = imageService;
            _recipeBookService = recipeBookService;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            string? userId = _userManager.GetUserId(User);

            var applicationDbContext = _context.Recipes
                                               .Include(r => r.RecipeBook);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                                       .Include(r => r.RecipeBook)
                                       .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public async Task<IActionResult> Create()
        {
            string? userId = _userManager.GetUserId(User)!;

            IEnumerable<RecipeBook> recipeBooksList = await _context.RecipeBooks
                                                                   .Where(r => r.RBUserId == userId)
                                                                   .ToListAsync();

            ViewData["RecipeBooksList"] = new SelectList(recipeBooksList, "Id", "Title");

            return View();
        }

        // POST: Recipes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RBUserId,RecipeBookId,Title,Description,Created,ImageFile")] Recipe recipe)
        {
            ModelState.Remove("RBUserId");

            if (ModelState.IsValid)
            {
                recipe.RBUserId = _userManager.GetUserId(User);
                recipe.Created = DateTime.UtcNow;

                if (recipe.ImageFile != null)
                {
                    recipe.ImageData = await _imageService.ConvertFileToByteArrayAsync(recipe.ImageFile);
                    recipe.ImageType = recipe.ImageFile.ContentType;
                }

                recipe.Created = DataUtility.GetPostGresDate(DateTime.UtcNow);

                _context.Add(recipe);
                await _context.SaveChangesAsync();

                //await _recipeBookService.AddRecipeToRecipeBookAsync(selected, recipe.Id);

                return RedirectToAction(nameof(Index));
            }

            ViewData["RBUserId"] = new SelectList(_context.Users, "Id", "Name", recipe.RBUserId);
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["RBUserId"] = new SelectList(_context.Set<RBUser>(), "Id", "Id", recipe.RBUserId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RBUserId,Title,Description,Created,ImageData,ImageType,ImageFile")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RBUserId"] = new SelectList(_context.Set<RBUser>(), "Id", "Id", recipe.RBUserId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.RBUserId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Recipes'  is null.");
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
          return (_context.Recipes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
