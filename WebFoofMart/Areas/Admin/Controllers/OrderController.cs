using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFoofMart.Models.DataModels;
using static NuGet.Packaging.PackagingConstants;

namespace WebFoofMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly FoodDbContext _Context;
        public OrderController(FoodDbContext context)
        {
            _Context = context;
        }
        public IActionResult Index(string searchName, int page = 1, int pageSize = 2)
        {
            // Lấy danh sách Orders dưới dạng IQueryable để phân trang
            var ListOrder = _Context.Orders.AsQueryable();

            // Lọc theo tên khách hàng nếu có
            if (!string.IsNullOrEmpty(searchName))
            {
                ListOrder = ListOrder.Where(p => p.FullName.ToLower().Contains(searchName.ToLower()));
            }

            // Đếm tổng số đơn hàng (sau khi lọc)
            var totalOrders = ListOrder.Count();

            // Lấy đơn hàng cho trang hiện tại
            var paginatedOrders = ListOrder
                .Skip((page - 1) * pageSize)  // Bỏ qua các đơn hàng của các trang trước
                .Take(pageSize)  // Lấy số đơn hàng tương ứng với pageSize
                .ToList();

            // Truyền dữ liệu phân trang về view
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalOrders / pageSize);  // Tính tổng số trang
            ViewBag.CurrentPage = page;  // Trang hiện tại
            ViewBag.SearchName = searchName;  // Giữ lại từ khóa tìm kiếm

            // Chỉ truyền danh sách đơn hàng đã phân trang
            ViewBag.order = paginatedOrders;  // Đây là danh sách đã phân trang

            return View();
        }


        public IActionResult Delete(int id)
        {
            // Tìm sản phẩm theo ID
            var order = _Context.Orders.Find(id);

            if (order != null)
            {
                // Xóa sản phẩm khỏi database
                _Context.Orders.Remove(order);
                _Context.SaveChanges();
            }

            // Sau khi xóa, quay lại trang danh sách sản phẩm
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult TogglePaymentStatus(int OrderId)
        {
            var order = _Context.Orders.Find(OrderId);
            if (order != null)
            {
                order.IsPaid = !order.IsPaid;
                _Context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
