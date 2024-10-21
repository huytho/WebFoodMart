using MessagePack;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFoofMart.Models.DataModels
{
    [Table("CartItems")]
    public class CartItem
    {
        public int CartId { get; set; }
        
        public String CartName { get; set; }
        public String Image { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public int  Soluong { get; set; }
        public double ThanhTien => Soluong * SalePrice;
    }

    
}
