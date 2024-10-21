using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFoofMart.Models.DataModels
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductId{ get; set; }

        [StringLength(100)]
        public string ProductName { get; set; } = null!;
        public double Price { get; set; }
        public double SalePrice { get; set; }

        [StringLength(256)]
        public string Image { get; set; } = String.Empty;

        public bool IsActive { get; set; } = true;
        public int IsDeleted { get; set; }
        public int? CategoryId { get; set; }

        public Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
