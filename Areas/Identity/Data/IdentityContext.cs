// Required namespaces for ASP.NET Core Identity and Entity Framework Core functionality.
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sashiel_CLDV6211_Part2.Models;

namespace Sashiel_CLDV6211_Part2.Areas.Identity.Data
{
    public class IdentityContext : IdentityDbContext
    {
        // Constructor that takes DbContextOptions and passes it to the base class constructor.
        // This is used to configure the context, for example, with a connection string or database provider.
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

         protected override void OnModelCreating(ModelBuilder builder)
        {
           
            base.OnModelCreating(builder);

            // You can customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        // A DbSet for the ApplicationUser entity, which is a custom user class extending IdentityUser.
        // This allows you to add additional properties to the user entity.
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        // A DbSet for the Products entity. This represents a collection of Products that will be mapped to a table in the database.
        // The `= default!;` ensures the property is initialized to avoid compiler warnings, especially in the context of nullable reference types.
        public DbSet<Products> Products { get; set; } = default!;

        // A DbSet for the PurchaseHistory entity. This represents the purchase history records.
        // Each entry in this DbSet corresponds to a record in the PurchaseHistory table in the database.
        public DbSet<PurchaseHistory> PurchaseHistory { get; set; }

        // A DbSet for the SalesStatement entity. This represents sales statements.
        // Each entry in this DbSet corresponds to a record in the SalesStatement table in the database.
        public DbSet<SalesStatement> SalesStatement { get; set; }

        // A DbSet for the CartOrders entity. This represents shopping cart orders.
        // Each entry in this DbSet corresponds to a record in the CartOrders table in the database.
        public DbSet<CartOrders> CartOrders { get; set; }
    }
}
