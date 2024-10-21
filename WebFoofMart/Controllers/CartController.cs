using Microsoft.AspNetCore.Mvc;
using WebFoofMart.Models;
using WebFoofMart.Models.DataModels;
using System.Web;
using Microsoft.AspNetCore.Http;
using WebFoofMart.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace WebFoofMart.Areas.Admin.Controllers
{
  
    public class CartController : Controller
    {
        private readonly FoodDbContext _context;
        public  CartController(FoodDbContext context)
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

        public IActionResult AddToCart(int id)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p=>p.CartId == id);
            if(item == null)
            {
                var CartSp = _context.Products.SingleOrDefault(p=>p.ProductId == id);
                item = new CartItem
                {
                           CartId = id,
                           CartName = CartSp.ProductName,
                           Price = CartSp.Price,
                           SalePrice = CartSp.SalePrice,
                           Soluong = 1,
                           Image = CartSp.Image,
                };
                myCart.Add(item);
            }
            else
            {
                item.Soluong++;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, myCart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveCart(int id)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.CartId == id);
            if(item != null)
            {
                myCart.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, myCart);
            }
            return RedirectToAction("Index");
        }

      
    // điều kiện khi thanh toán chưa đăng nhập sẽ vào login
            public IActionResult CheckOut(string fullname, string email)
            {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
            {
                // Chuyển hướng đến trang đăng nhập và ghi nhớ URL hiện tại
                return RedirectToAction("Login", "Home" ,new { returnUrl = Url.Action("CheckOut", "Cart") });
            }
            // Lấy thông tin người dùng từ session hoặc database
            var username = HttpContext.Session.GetString("username");

            // Giả sử bạn có một bảng Users để lưu thông tin người dùng
            var user = _context.Accounts.FirstOrDefault(u => u.Username == username);


            // Kiểm tra nếu giỏ hàng rỗng
            if (Carts == null || !Carts.Any())
            {
                return RedirectToAction("Index"); // Quay lại trang chủ hoặc trang giỏ hàng
            }
            // Nếu tìm thấy người dùng, truyền thông tin người dùng sang View
            if (user != null)
            {
                ViewBag.FullName = user.Fullname;
                ViewBag.Email = user.Email;
                ViewBag.Address = user.Address;
                ViewBag.Phone = user.phone;
            }
            return View(Carts);
               
             }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
