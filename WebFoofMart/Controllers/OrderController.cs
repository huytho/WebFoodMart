using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFoofMart.Helpers;
using WebFoofMart.Models.DataModels;

namespace WebFoofMart.Controllers
{
    public class OrderController : Controller
    {
        private readonly FoodDbContext _context;
        public OrderController(FoodDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(Carts);
        }
        public List<CartItem> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY);
                if (data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }
        [HttpPost]
        public IActionResult Checkout(string fullname, string email, string address, string phone)
        {
            // Xử lý dữ liệu người mua
            if (string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(phone))
            {
                ViewBag.error = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }
            string productNames = string.Join(", ", Carts.Select(c => c.CartName));
            string productImages = string.Join(", ", Carts.Select(c => c.Image));

            // Logic thanh toán và lưu thông tin người mua vào cơ sở dữ liệu
            Order newOrder = new Order
            {
                FullName = fullname,
                Email = email,
                Address = address,
                Phone = phone,
                 TotalPrice = Carts.Sum(item => item.Soluong * item.SalePrice),
                 ProductNames= productNames,
                ProductImages = productImages,
                IsPaid = true
                // Thêm các thông tin khác cho đơn hàng
            };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // Sau khi thanh toán thành công, chuyển hướng đến trang cảm ơn hoặc trang khác
            return RedirectToAction("OrderSuccess");



        }
        public IActionResult OrderSuccess()
        {
            return View();
        }
 
    }
}
