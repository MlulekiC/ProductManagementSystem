using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ProductManagementSystemContext _context;

        public CategoriesController(ProductManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            int? userID = this.HttpContext.Session.GetInt32("UserID");
            try
            {
                var results = await _context.Categories
                    .Where(c => c.CreateUser == userID)
                    .ToListAsync();

                return View(results);
            }
            catch
            {
                throw;
            }
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _context.Categories
                    .Where(c => c.CategoryID == id)
                    .FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            int? userID = this.HttpContext.Session.GetInt32("UserID");
            if (ModelState.IsValid)
            {
                // Get the last Category ID
                var lastID = _context.Categories.Max(c => c.CategoryID);

                // Set the remaining object properties
                category.CategoryID = (lastID.ToString() != null ? lastID : 0) + 1;
                category.CreateDate = DateTime.Now;
                category.CreateUser = userID;
                category.Status = "A";

                var results = _context.Add(category);

                //if successfully added, commit changes
                if (results.State.ToString() == "Added")
                {
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mainCategory = await _context.Categories.FindAsync(id);
                    mainCategory.CategoryCode = category.CategoryCode;
                    mainCategory.Name = category.Name;
                    mainCategory.AmendDate = DateTime.Now;

                    _context.Update(mainCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryID))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
        }
    }
}
