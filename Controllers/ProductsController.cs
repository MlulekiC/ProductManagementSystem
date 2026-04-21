using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;
using System.Xml.Linq;

namespace ProductManagementSystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductManagementSystemContext _context;
        private readonly IWebHostEnvironment? _env;

        public ProductsController(ProductManagementSystemContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _env = webHost;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 10;

            int? userID = this.HttpContext.Session.GetInt32("UserID");
            try
            {
                var products = from product in _context.Products
                              join category in _context.Categories on product.CategoryID equals category.CategoryID
                              where product.CreateUser == userID
                              select new Product
                              {
                                  ProductID = product.ProductID,
                                  ProductName = product.ProductName,
                                  ProductCode = product.ProductCode,
                                  Description = product.Description,
                                  CategoryID = product.CategoryID,
                                  Category = category.Name,
                                  Price = product.Price,
                                  Image = product.Image,
                                  CreateUser = product.CreateUser
                              };
                return View(await Utilities.Helpers.PaginatedList<Product>.CreateAsync(products, pageNumber ?? 1, pageSize));
            }
            catch
            {
                throw;
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Where(p => p.ProductID == id)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {//Get logged in User ID
            int? userID = this.HttpContext.Session.GetInt32("UserID");
            Product product = new Product();
            var results = await _context.Categories
                    .Where(c => c.CreateUser == userID)
                    .ToListAsync();
            product.Categories = results;
            return View(product);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormCollection form)
        {
            //Get logged in User ID
            int? userID = this.HttpContext.Session.GetInt32("UserID");
            if (ModelState.IsValid)
            {
                var lastID = _context.Products.Max(p => p.ProductID);

                //Set remaining object properties
                product.ProductID = (lastID.ToString() != null ? lastID : 0) + 1;
                product.ProductCode = GenerateProductCode();
                product.CreateDate = DateTime.Now;
                product.CreateUser = userID;
                if (product.ImageFile != null)
                {
                    product.Image = await SaveProductImage(product);
                }

                var results = await _context.Products.AddAsync(product);

                //if successfully added, commit changes
                if (results.State.ToString() == "Added")
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "Error Adding new product, please tr again";
                }
            }
            return View();
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            int? userID = this.HttpContext.Session.GetInt32("UserID");
            Product? product = null;
            if (id == null)
            {
                return NotFound();
            }
            // Retrieve the select Product
            product = await _context.Products
                .Where(p => p.ProductID == id)
                .FirstOrDefaultAsync();

            // Retrieve all categories belonging to the current user
            var categories = await _context.Categories
                    .Where(c => c.CreateUser == userID)
                    .ToListAsync();
            product.Categories = categories;
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mainProduct = await _context.Products.FindAsync(id);

                    mainProduct.ProductName = product.ProductName;
                    mainProduct.AmendDate = DateTime.Now;
                    mainProduct.Description = product.Description;
                    mainProduct.Image = product.Image;
                    mainProduct.CategoryID = product.CategoryID;
                    mainProduct.Price = product.Price;

                    _context.Update(mainProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }

        private string GenerateProductCode()
        {
            DateTime now = DateTime.Now;
            string currentMonth = now.ToString("yyyyMM");
            string file = _env!.WebRootPath + "\\" + "Config.xml";
            string lastMonth = "";
            int lastSeq = 0;

            try
            {
                XDocument doc = XDocument.Load(file);
                var month = doc.Root!.Element("lastGeneratedMonth")!.Value;
                var sequenceValue = doc.Root.Element("lastSequenceValue")!.Value;

                if (month != null && sequenceValue != null)
                {
                    lastMonth = string.IsNullOrEmpty(month) ? currentMonth : month;
                    lastSeq = string.IsNullOrEmpty(sequenceValue) ? 0 : int.Parse(sequenceValue);
                }
                if (currentMonth != lastMonth)
                {
                    lastSeq = 1; 
                    lastMonth = currentMonth;
                }
                else
                {
                    lastSeq++;
                }
                UpdateConfigFile(lastMonth, $"{lastSeq:D3}");
                return $"{lastMonth}-{lastSeq:D3}";
            }
            catch
            {
                throw;
            }
        }

        private void UpdateConfigFile(string Month, string LastSequence)
        {
            string file = _env!.WebRootPath + "\\" + "Config.xml";

            try
            {
                XDocument doc = XDocument.Load(file);
                if (doc != null)
                {
                    var month = doc.Root.Element("lastGeneratedMonth");
                    var SeqValue = doc.Root.Element("lastSequenceValue");

                    month.Value = Month;
                    SeqValue.Value = LastSequence;
                }
                doc.Save(file);
            }
            catch
            {
                throw;
            }
        }
        private async Task<string> SaveProductImage(Product product)
        {
            string fileName = "";
            string filePath = "";
            string folder = _env!.WebRootPath + "\\" + "images";
            if (product.ImageFile != null)
            {
                // Create a unique filename to prevent overwriting
                fileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                filePath = Path.Combine(folder, fileName);

                // Copy the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }
            }
            return filePath;
        }
    }

    
}
