using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFoofMart.Models.DataModels
{
    [Table("Invoices")]
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime NgayDat { get; set; } = DateTime.Now;
        public DateTime NgayDao { get; set; } = DateTime.Now;
        public DateTime NgayCan { get; set; } = DateTime.Now;

        public string CachThanhToan { get; set; } = null!;
        public string CachVanChuyen { get; set; } = null!;
        public decimal TotalAmount { get; set; }

        public string? AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account  Account{ get; set; }

    }
}
