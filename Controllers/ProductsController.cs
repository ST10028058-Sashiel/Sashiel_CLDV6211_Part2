using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Sashiel_CLDV6211_Part2.Areas.Identity.Data;
using Sashiel_CLDV6211_Part2.Models;
using Microsoft.AspNetCore.Identity;


namespace Sashiel_CLDV6211_Part2.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IdentityContext _context;

        Products newProducts = new Products();

        public ProductsController(IdentityContext context)
        {
            _context = context;
        }

        
        public IActionResult MyWork()
        {
            try
            {
                List<Products> products = _context.Products.ToList();
                Console.WriteLine("Number of products retrieved: " + products.Count); // Add this line for debugging
                return View(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }



      
        [Authorize]
        public IActionResult Cart()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var cartItems = _context.CartOrders
                                    .Include(c => c.Product)
                                    .Where(c => c.UserId == userId)
                                    .ToList();
            return View(cartItems);
        }

      
        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Product_ID == productId);
            if (product == null)
            {
                return NotFound();
            }

            if (product.Quantity <= 0)
            {
                TempData["ErrorMessage"] = "Product is not available.";
                return RedirectToAction(nameof(MyWork));
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var cartOrder = new CartOrders
            {
                Product_ID = productId,
                UserId = userId
            };

            _context.CartOrders.Add(cartOrder);
            product.Quantity--;
            product.Availability = product.Quantity > 0;

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product added to cart successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while adding the product to the cart: {ex.Message}";
            }

            return RedirectToAction(nameof(Cart));
        }

      
        [HttpPost]
        [Authorize]
        public IActionResult RemoveFromCart(int cartOrderId)
        {
            var cartOrder = _context.CartOrders.Include(c => c.Product).FirstOrDefault(c => c.Cart_ID == cartOrderId);
            if (cartOrder == null)
            {
                return NotFound();
            }

            _context.CartOrders.Remove(cartOrder);
            cartOrder.Product.Quantity++;
            cartOrder.Product.Availability = true;

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product removed from cart successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while removing the product from the cart: {ex.Message}";
            }

            return RedirectToAction(nameof(Cart));
        }

    
        [HttpPost]
        [Authorize]
        public IActionResult Pay()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var cartItems = _context.CartOrders
                                    .Include(c => c.Product)
                                    .Where(c => c.UserId == userId)
                                    .ToList();

            foreach (var item in cartItems)
            {
                var salesStatement = new SalesStatement
                {
                    Product_ID = item.Product_ID,
                    UserId = userId,
                    Purchase_Date = DateTime.Now,
                    Status = "Pending"
                };

                _context.SalesStatement.Add(salesStatement);
                _context.CartOrders.Remove(item);
            }

            _context.SaveChanges();

            
            TempData["OrderPlacedMessage"] = "Payment pending. Order placed successfully.";
            return RedirectToAction(nameof(Cart));
        }

 
        [Authorize]
        public IActionResult History()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var transactions = _context.SalesStatement
                                       .Where(s => s.UserId == userId)
                                       .Include(s => s.Product)
                                       .ToList();
            return View("AccessControl/History", transactions);
        }
    }
}
