using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly ProductManagementSystemContext _context;

        public UsersController(ProductManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User? user)
        {
            User? insUser = new User();
            insUser = await _context.Users
                    .Where(u => u.UserName == user!.UserName)
                    .Select(r => new User
                    {
                        UserId = r.UserId,
                        UserName = r.UserName,
                        Password = r.Password,
                    })
                    .FirstOrDefaultAsync();
            if (insUser != null)
            {
                // Check if password matches
                if (Utilities.Helpers.Decrypt(insUser!.Password) == user!.Password)
                {
                    this.HttpContext.Session.SetInt32("UserID", insUser.UserId);
                    return RedirectToAction(nameof(Index), "Products");
                }
                ViewBag.Error = "Password incorrect, please try again.";
            }
            ViewBag.Error = "Could not find your account, please verify your details and try again.";
            return View(nameof(Index));
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                dynamic cont = HttpContext.Connection;
                string UserDetails = user.UserName + ":" + user.Password;
                string encryptedDetails = Utilities.Helpers.Encrypt(UserDetails);

                var url = HttpContext.Request.Scheme + "/" + HttpContext.Request.Host + "/" + "SyncQueue/Confirmation/" + encryptedDetails;
                if (await Utilities.Email.SendEmailAsync(url, user.UserName))
                {

                    #region Sync Queue Functionality
                    //var lastQueueID = _context.SyncQueue.Max(s => s.QueueID);
                    /*
                     There was goin to be the code for writing into the Synq Queue here, but I could not setup IIS
                    So for now will insert directly into Users table
                     */
                    #endregion Sync Queue Functionality

                    var lastID = _context.Users.Max(u => u.UserId);

                    user.UserId = (lastID.ToString() != null ? lastID : 0) + 1;
                    user.Password = Utilities.Helpers.Encrypt(user.Password);
                    user.Status = "A";
                    user.CreateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    user.CreateUser = 1;
                    var results = await _context.Users.AddAsync(user);

                    // If user record inserted, create security logins and commit changes
                    if (results.State.ToString() == "Added")
                    {
                        CreateUser(user.UserName, user.Password);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ViewBag.Error = "Error creating user records, please try again";
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            return View(user);
        }

        private bool CreateUser(string username, string password)
        {
            try
            {
                // Create User Login (Server Level)
                string loginSql = $"CREATE LOGIN [{username}] WITH PASSWORD = '{password}'";
                _context.Database.ExecuteSqlRaw(loginSql);

                // Create User (Database Level)
                string userSql = $"CREATE USER [{username}] FOR LOGIN [{username}]";
                _context.Database.ExecuteSqlRaw(userSql);

                // Grant Roles
                string roleSql = $"ALTER ROLE db_datawriter ADD MEMBER [{username}]";
                _context.Database.ExecuteSqlRaw(roleSql);

                roleSql = $"ALTER ROLE db_datareader ADD MEMBER [{username}]";
                _context.Database.ExecuteSqlRaw(roleSql);
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return true;
        }
    }
}
