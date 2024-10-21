using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFoofMart.Models.DataModels
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        // Liên kết với Order
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Liên kết với sản phẩm
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Thông tin sản phẩm trong đơn hàng
        public int Quantity { get; set; }
        public double SalePrice { get; set; }
        public double TotalPrice => Quantity * SalePrice; // Thành tiền
    }

}
