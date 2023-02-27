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
using ContactPro.Services;

namespace BWRecipeBook.Controllers
{
    [Authorize]
    public class RecipeBooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<RBUser> _userManager;
        private readonly IImageService _imageService;

        public RecipeBooksController(ApplicationDbContext context,
                                     UserManager<RBUser> userManager,
                                     IImageService imageService)
        {
            _context = context;
            _userManager = userManager;
            _imageService = imageService;
        }

        // GET: RecipeBooks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RecipeBooks.Include(r => r.RBUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RecipeBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RecipeBooks == null)
            {
                return NotFound();
            }

            var recipeBook = await _context.RecipeBooks
                                           .Include(r => r.RBUser)
                                           .FirstOrDefaultAsync(r => r.Id == id);
            if (recipeBook == null)
            {
                return NotFound();
            }

            return View(recipeBook);
        }

        // GET: RecipeBooks/Create
        public async Task<IActionResult> Create()
        {
            string? userId = _userManager.GetUserId(User);

            IEnumerable<RecipeBook> recipeBookList = await _context.RecipeBooks
                                                                   .Where(r => r.RBUserId == userId)
                                                                   .ToListAsync();

            ViewData["RecipeBooksList"] = new SelectList(recipeBookList, "Id", "Title");

            return View();
        }

        // POST: RecipeBooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RBUserId,Title,Description")] RecipeBook recipeBook)
        {
            ModelState.Remove("RBUserId");

            if (ModelState.IsValid)
            {
                recipeBook.RBUserId = _userManager.GetUserId(User);

                _context.Add(recipeBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(recipeBook);
        }

        // GET: RecipeBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RecipeBooks == null)
            {
                return NotFound();
            }

            var recipeBook = await _context.RecipeBooks.FindAsync(id);
            if (recipeBook == null)
            {
                return NotFound();
            }
            ViewData["RBUserId"] = new SelectList(_context.Set<RBUser>(), "Id", "Id", recipeBook.RBUserId);
            return View(recipeBook);
        }

        // POST: RecipeBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] RecipeBook recipeBook)
        {
            if (id != recipeBook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeBookExists(recipeBook.Id))
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
            ViewData["RBUserId"] = new SelectList(_context.Set<RBUser>(), "Id", "Id", recipeBook.RBUserId);
            return View(recipeBook);
        }

        // GET: RecipeBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RecipeBooks == null)
            {
                return NotFound();
            }

            var recipeBook = await _context.RecipeBooks
                .Include(r => r.RBUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeBook == null)
            {
                return NotFound();
            }

            return View(recipeBook);
        }

        // POST: RecipeBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RecipeBooks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RecipeBooks'  is null.");
            }
            var recipeBook = await _context.RecipeBooks.FindAsync(id);
            if (recipeBook != null)
            {
                _context.RecipeBooks.Remove(recipeBook);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeBookExists(int id)
        {
          return (_context.RecipeBooks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
