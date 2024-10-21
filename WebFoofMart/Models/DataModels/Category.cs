using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFoofMart.Models.DataModels
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int  CategoryId{ get; set; }
        [StringLength(100)]
        public string CategoryName { get; set; } = null!;
        public int IsDeleted { get; set; }
        public ICollection<Product> Products { get; set;}
    }
}
