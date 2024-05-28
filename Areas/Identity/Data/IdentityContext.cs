using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sashiel_CLDV6211_Part2.Models;

namespace Sashiel_CLDV6211_Part2.Areas.Identity.Data
{
    public class IdentityContext : IdentityDbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

		}

		
		
		public DbSet<ApplicationUser> ApplicationUser { get; set; } = default!;
		public DbSet<Products> Products { get; set; } = default!;
		public DbSet<PurchaseHistory> PurchaseHistory { get; set; }		
		public DbSet<SalesStatement> SalesStatement { get; set; }	
		public DbSet<CartOrders> CartOrders { get; set; }
	}
}
