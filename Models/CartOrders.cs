using Sashiel_CLDV6211_Part2.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sashiel_CLDV6211_Part2.Models
{
    public class CartOrders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cart_ID { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int Product_ID { get; set; } /

        [Required]
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }

        public virtual Products Product { get; set; } 
        public virtual ApplicationUser User { get; set; }
    }
}
