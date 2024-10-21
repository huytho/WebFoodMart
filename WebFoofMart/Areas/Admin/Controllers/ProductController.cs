using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFoofMart.Models.DataModels;

namespace WebFoofMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
  private readonly FoodDbContext _Context;
        public ProductController (FoodDbContext context)
        {
            _Context = context;
        }
        public IActionResult Index(string searchName, int page = 1, int pageSize = 5)
        {
            var Listpro = _Context.Products.Include(p => p.Category).AsQueryable(); // Lấy thông tin Category

            if (!string.IsNullOrEmpty(searchName))
            {
                Listpro = Listpro.Where(p => p.ProductName.ToLower().Contains(searchName.ToLower()));
            }

            var totalProducts = Listpro.Count();

            // Lấy sản phẩm cho trang hiện tại
            var paginatedProducts = Listpro
                .Skip((page - 1) * pageSize) // Bỏ qua các sản phẩm ở trang trước
                .Take(pageSize) // Lấy số sản phẩm tương ứng với pageSize
                .ToList();

            // Truyền dữ liệu phân trang về view
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize); // Tổng số trang
            ViewBag.CurrentPage = page; // Trang hiện tại

            // Truyền danh sách sản phẩm và từ khóa tìm kiếm về View
            ViewBag.SearchName = searchName;
            ViewBag.pro = paginatedProducts;

            return View();
        }


        /*       public IActionResult Index(string searchName, int page = 1, int pageSize = 5)
               {

                   var Listpro = _Context.Products.Include(p => p.Category).AsQueryable();// Lấy thông tin Category
                   if (!string.IsNullOrEmpty(searchName))
                   {

                       Listpro = Listpro.Where(p => p.ProductName.ToLower().Contains(searchName.ToLower()));
                   }
                   var totalProducts = Listpro.Count();

                   // Lấy sản phẩm cho trang hiện tại
                   var paginatedProducts = Listpro
                       .Skip((page - 1) * pageSize) // Bỏ qua các sản phẩm ở trang trước
                       .Take(pageSize) // Lấy số sản phẩm tương ứng với pageSize
                       .ToList();

                   // Truyền dữ liệu phân trang về view
                   ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize); // Tổng số trang
                   ViewBag.CurrentPage = page; // Trang hiện tại
                   // Truyền danh sách sản phẩm và từ khóa tìm kiếm về View
                   ViewBag.SearchName = searchName;
                   ViewBag.pro = paginatedProducts;

                   return View();
               }*/

        public IActionResult Create()
        {
            // Lấy danh sách các Category để hiển thị trong dropdown
            ViewBag.Categories = _Context.Categories.ToList();
            return View();
        }
        // Xử lý dữ liệu từ form để thêm mới Product
        [HttpPost]
        public IActionResult Save(Product product)
        {

                _Context.Products.Add(product);
                _Context.SaveChanges();

            return RedirectToAction("Index");

        }
        public IActionResult Delete(int id)
        {
            // Tìm sản phẩm theo ID
            var product = _Context.Products.Find(id);

            if (product != null)
            {
                // Xóa sản phẩm khỏi database
                _Context.Products.Remove(product);
                _Context.SaveChanges();
            }

            // Sau khi xóa, quay lại trang danh sách sản phẩm
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            // Lấy thông tin sản phẩm từ database
            var product = _Context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu của sản phẩm và danh sách các category để hiển thị trong dropdown
            ViewBag.Categories = _Context.Categories.ToList();
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            // Tìm sản phẩm trong database
            var EditProduct = _Context.Products.Find(product.ProductId);

            if (EditProduct != null)
            {
                // Cập nhật các trường thông tin
                EditProduct.ProductName = product.ProductName;
                EditProduct.Price = product.Price;
                EditProduct.SalePrice = product.SalePrice;
                EditProduct.Image = product.Image;
                EditProduct.CategoryId = product.CategoryId;

                // Lưu thay đổi vào database
                _Context.SaveChanges();
            }

            // Chuyển hướng về trang danh sách sau khi cập nhật thành công
            return RedirectToAction("Index");
        }
    }
}
