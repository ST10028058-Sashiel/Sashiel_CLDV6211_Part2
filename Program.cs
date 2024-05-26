using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sashiel_CLDV6211_Part2.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Sashiel_CLDV6211_Part2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.")));

            builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultTokenProviders().
                AddRoles<IdentityRole>().
                AddEntityFrameworkStores<IdentityContext>();



            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Add this line to enable authentication
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.Run();
        }
    }
}