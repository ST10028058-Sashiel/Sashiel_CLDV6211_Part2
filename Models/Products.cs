using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sashiel_CLDV6211_Part2.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Product_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Product_Name { get; set; }

        public string Product_Description { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Product_Price { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        public bool Availability { get; set; }
    }
}
