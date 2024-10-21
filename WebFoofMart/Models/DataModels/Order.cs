using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFoofMart.Models.DataModels
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [StringLength(100)]
        public string FullName { get; set; } = null!; // Tên người mua
        [StringLength(100)]
        public string Email { get; set; } = null!;    // Email người mua
        [StringLength(10)]
        public  string Phone { get; set; } = null!;// sđt
        [StringLength(250)]
        public string Address { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.Now;//

        // Tổng giá trị đơn hàng
        public double TotalPrice { get; set; }

        // Trạng thái đơn hàng (đã thanh toán hay chưa)
        public bool IsPaid { get; set; }
        public string? AccountId { get; set; }

        public Account Account { get; set; } = null!;
        // Liên kết với các OrderDetails
        public string ProductNames { get; set; } = string.Empty;
        public string ProductImages { get; set; } = string.Empty;
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
 
    }

}
