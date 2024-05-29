using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sashiel_CLDV6211_Part2.Models;
using Microsoft.AspNetCore.Identity;

namespace Sashiel_CLDV6211_Part2.Models
{
    public class ProductStatement
    {
        [Key]
        public int Statement_ID { get; set; }

        [ForeignKey("Product")]
        public int Product_ID { get; set; }

        public DateTime Purchase_Date { get; set; }

        [Required]
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Pending";

        public Products? Product { get; set; }

        public IdentityUser? User { get; set; }
    }
}
