using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebFoofMart.Models.DataModels
{
    [Table("Accounts")]
        public class Account
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Column(TypeName = "varchar"), StringLength(36)]
            public string AccountId { get; set; }
            [StringLength(100)]
        [Required]
        public string Fullname { get; set; } = null!;
        [StringLength(100)]
        [Required]
        public string Username { get; set; } = null!;
                    [Required]
            [StringLength(256), Column(TypeName = "varchar")]
  
         public string Password { get; set; } = null!;
        [Required]
        [StringLength (256),Column(TypeName = "nvarchar")]
            public string  Address { get; set; } = null!;
        [Required]
        [Phone]
        public string  phone { get; set; } = null!;
        [Required]
        [EmailAddress]
            public string Email { get; set; } = null!;
            public bool IsAdmin { get; set; } = true;
            public string? Picture { get; set; } = string.Empty;
            public bool IsActive { get; set; } = true;
            public ICollection<Order> Orders { get; set;}
        }
}
