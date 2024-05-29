using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sashiel_CLDV6211_Part2.Models;
using Sashiel_CLDV6211_Part2.Areas.Identity.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sashiel_CLDV6211_Part2.Controllers
{
    // The AccountController handles operations related to accounts and products.
    // It uses ASP.NET Core Identity for authorization and Entity Framework Core for data access.
    public class AccountController : Controller
    {
        // Private field to hold the IdentityContext instance for database operations.
        private readonly IdentityContext identityContext;

        // Constructor to inject the IdentityContext instance via dependency injection.
        public AccountController(IdentityContext context)
        {
            identityContext = context;
        }

        // GET: /Account/Index
        // This action returns a view displaying a list of products.
        // Only accessible to users in the "Admin" role.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            // Fetch all products from the database asynchronously.
            List<Products> products = await identityContext.Products.ToListAsync();
            return View(products);
        }

        // GET: /Account/Details/{id}
        // This action returns a view displaying details of a specific product by its ID.
        // Only accessible to users in the "Admin" role.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the product with the specified ID from the database.
            Products product = await identityContext.Products
                .FirstOrDefaultAsync(m => m.Product_ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: /Account/Create
        // This action returns a view for creating a new product.
        // Only accessible to users in the "Admin" role.
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Account/Create
        // This action handles the POST request to create a new product.
        // Only accessible to users in the "Admin" role.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Product_ID,Product_Name,Product_Description,Product_Price,ImagePath,Quantity")] Products products)
        {
            if (ModelState.IsValid)
            {
                // Set the availability of the product based on its quantity.
                products.Availability = products.Quantity > 0;
                // Add the new product to the context and save changes asynchronously.
                identityContext.Add(products);
                await identityContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        // GET: /Account/Edit/{id}
        // This action returns a view for editing an existing product by its ID.
        // Only accessible to users in the "Admin" role.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the product with the specified ID from the database.
            Products product = await identityContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: /Account/Edit/{id}
        // This action handles the POST request to update an existing product.
        // Only accessible to users in the "Admin" role.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Product_ID,Product_Name,Product_Description,Product_Price,ImagePath,Quantity,Availability")] Products products)
        {
            if (id != products.Product_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the availability of the product based on its quantity.
                    products.Availability = products.Quantity > 0;
                    // Update the product in the context and save changes asynchronously.
                    identityContext.Update(products);
                    await identityContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Product_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        // GET: /Account/Delete/{id}
        // This action returns a view for deleting an existing product by its ID.
        // Only accessible to users in the "Admin" role.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the product with the specified ID from the database.
            Products product = await identityContext.Products
                .FirstOrDefaultAsync(m => m.Product_ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: /Account/Delete/{id}
        // This action handles the POST request to confirm the deletion of a product.
        // Only accessible to users in the "Admin" role.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the product with the specified ID and remove it from the context.
            Products product = await identityContext.Products.FindAsync(id);
            if (product != null)
            {
                identityContext.Products.Remove(product);
            }

            // Save changes asynchronously.
            await identityContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a product with a specified ID exists.
        private bool ProductsExists(int id)
        {
            return identityContext.Products.Any(e => e.Product_ID == id);
        }
    }
}
