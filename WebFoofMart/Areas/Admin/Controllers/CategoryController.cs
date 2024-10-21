using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebFoofMart.Models.DataModels;

namespace WebFoofMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly FoodDbContext _Context;
        public CategoryController(FoodDbContext context)
        {
            _Context = context;
        }
        public IActionResult Index(string searchName, int page = 1, int pageSize = 2)
        {
            // Lấy danh sách Category dưới dạng IQueryable để phân trang
            var ListCate = _Context.Categories.AsQueryable();

            // Lọc theo tên khách hàng nếu có
            if (!string.IsNullOrEmpty(searchName))
            {
                ListCate = ListCate.Where(p => p.CategoryName.ToLower().Contains(searchName.ToLower()));
            }

            // Đếm tổng số đơn hàng (sau khi lọc)
            var totalCate= ListCate.Count();

            // Lấy đơn hàng cho trang hiện tại
            var paginatedOrders = ListCate
                .Skip((page - 1) * pageSize)  // Bỏ qua các đơn hàng của các trang trước
                .Take(pageSize)  // Lấy số đơn hàng tương ứng với pageSize
                .ToList();

            // Truyền dữ liệu phân trang về view
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCate / pageSize);  // Tính tổng số trang
            ViewBag.CurrentPage = page;  // Trang hiện tại
            ViewBag.SearchName = searchName;  // Giữ lại từ khóa tìm kiếm

            // Chỉ truyền danh sách đơn hàng đã phân trang
            ViewBag.cate = paginatedOrders;  // Đây là danh sách đã phân trang

            return View();
        }

        public IActionResult Delete(int id)
        {
            // Tìm sản phẩm theo ID
            var cate = _Context.Categories.Find(id);

            if (cate != null)
            {
                // Xóa sản phẩm khỏi database
                _Context.Categories.Remove(cate);
                _Context.SaveChanges();
            }

            // Sau khi xóa, quay lại trang danh mục sản phẩm
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            // Lấy danh sách các Category để hiển thị trong dropdown
            return View();
        }
        [HttpPost]
        public IActionResult Save(Category category)
        {

            _Context.Categories.Add(category);
            _Context.SaveChanges();

            return RedirectToAction("Index");

        }
        public IActionResult Edit(int id)
        {
           
          var category = _Context.Categories.FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
            {
                // Nếu không tìm thấy danh mục, trả về lỗi hoặc chuyển hướng
                return NotFound();
            }

            // Truyền dữ liệu danh mục vào view để hiển thị
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            // Tìm sản phẩm trong database
            var EditCategory = _Context.Categories.Find(category.CategoryId);

            if (EditCategory != null)
            {
                // Cập nhật các trường thông tin
                EditCategory.CategoryId = category.CategoryId;
                EditCategory.CategoryName = category.CategoryName;

                // Lưu thay đổi vào database
                _Context.SaveChanges();
            }

            // Chuyển hướng về trang danh sách sau khi cập nhật thành công
            return RedirectToAction("Index");
        }
    }
}
