using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sashiel_CLDV6211_Part2.Models;
using Microsoft.AspNetCore.Identity;

namespace Sashiel_CLDV6211_Part2.Models
{
    public class PurchaseHistory
    {
        [Key]
        public int History_ID { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int Product_ID { get; set; }

        [Required]
        public DateTime PDate { get; set; }

        [Required]
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Pending";

        public Products? Product { get; set; }

        public IdentityUser? User { get; set; }
    }
}
