using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Data;

namespace ProductManagementSystem.Controllers
{
    public class SyncQueueController : Controller
    {
        private readonly ProductManagementSystemContext _context;

        public SyncQueueController(ProductManagementSystemContext context)
        {
            _context = context;
        }
        // GET: SyncQueue
        public ActionResult Index()
        {
            try
            {
                dynamic httpContext = HttpContext.Connection;
            }
            catch
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                dynamic cont = HttpContext.Connection;
                var vv = HttpContext.Request.Path;
                _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: SyncQueue/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SyncQueue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SyncQueue/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SyncQueue/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SyncQueue/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SyncQueue/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
