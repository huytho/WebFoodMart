using Microsoft.AspNetCore.Mvc;
using WebFoofMart.Helpers;
using WebFoofMart.Models.DataModels;
namespace WebFoofMart.ViewComponents
{
    public class CartViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart =  HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
            return View("CartPanel", new CartModel
            {
                Quantity  = cart.Sum(p=>p.Soluong),
                Total = cart.Sum(p=>p.ThanhTien)
            });
        }
    }
}
