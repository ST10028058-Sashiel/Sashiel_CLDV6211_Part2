using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sashiel_CLDV6211_Part2.Models;
using Sashiel_CLDV6211_Part2.Areas.Identity.Data;

namespace Sashiel_CLDV6211_Part2.Controllers
{
    public class AccountController : Controller
    {
        private readonly IdentityContext _context;

        public AccountController(IdentityContext context)
        {
            _context = context;
        }

 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

    
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Product_ID == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

  
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Product_ID,Product_Name,Product_Description,Product_Price,ImagePath,Quantity")] Products products)
        {
            if (ModelState.IsValid)
            {
                products.Availability = products.Quantity > 0;
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

    
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        
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
                    products.Availability = products.Quantity > 0;
                    _context.Update(products);
                    await _context.SaveChangesAsync();
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


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Product_ID == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            if (products != null)
            {
                _context.Products.Remove(products);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Product_ID == id);
        }
    }
}
