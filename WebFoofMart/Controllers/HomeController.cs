using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using WebFoofMart.Models;
using WebFoofMart.Models.DataModels;

namespace WebFoofMart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private FoodDbContext _context;
        public HomeController(ILogger<HomeController> logger , FoodDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int? categoryId)
        {
            List<Product> listPro;

            // Kiểm tra nếu categoryId có giá trị, lọc theo CategoryId
            if (categoryId.HasValue)
            {
                listPro = _context.Products
                                  .Where(p => p.CategoryId == categoryId.Value)
                                  .ToList();
            }
            else
            {
                listPro = _context.Products.ToList(); // Lấy tất cả sản phẩm
            }

            ViewBag.poo = listPro;
            ViewBag.CategoryId = categoryId; // Truyền CategoryId để biết tab nào đang active
            ViewBag.Categories = _context.Categories.ToList(); // Lấy danh sách danh mục
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; // Lưu giá trị returnUrl để chuyển lại sau khi đăng nhập
            return View();
        }
        public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult Login(string username, string password, string returnUrl = null)
    {
        var account = _context.Accounts.SingleOrDefault(x => x.Username == username);
        var passmd5 = Cipher.GenerateMD5(password);
        if (account == null)
        {
            ViewBag.error = "<div class='alert alert-danger'>Đăng nhập sai</div>";
            return View();
        }
        else
        {
            if (!account.Password.Equals(passmd5))
            {
                ViewBag.error = "<div class='alert alert-danger'>Đăng nhập sai</div>";
                return View();
            }
            else
            {
                HttpContext.Session.SetString("fullname", account.Fullname);
                HttpContext.Session.SetString("picture", account.Picture);
                HttpContext.Session.SetString("username", account.Username);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                }
        }
        return RedirectToAction("Index");
    }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string fullname, string email, string address, string phone,string password,string username)
        {
        
                // Check if the username already exists
                if (string.IsNullOrEmpty(username) ||string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(phone)|| string.IsNullOrEmpty(password))
                {
                    ViewBag.error = "Vui lòng nhập đầy đủ thông tin!";
                    return View();
                }
                // Create new account
                var newAccount = new Account
                {
                    Username = username,
                    Fullname = fullname,
                    Email = email,
                    Address = address,
                    phone = phone,
                    Password = Cipher.GenerateMD5(password), // Hash the password
                    IsAdmin = false, // By default, new users are not admins
                    IsActive = true // Active by default
                };

                // Add account to the database
                _context.Accounts.Add(newAccount);
                _context.SaveChanges();

                // Redirect to the homepage or a welcome page
                return RedirectToAction("Login", "Home");
            }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}