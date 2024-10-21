using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebFoofMart.Models.DataModels;

namespace WebFoofMart.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private FoodDbContext _context;
        public ProductController(ILogger<ProductController> logger, FoodDbContext context)
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




    }

}
