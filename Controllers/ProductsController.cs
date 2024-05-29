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
       
        private readonly IdentityContext identityContext;

  
        Products newProducts = new Products();

        // Constructor to inject the IdentityContext instance via dependency injection.
        public ProductsController(IdentityContext context)
        {
            identityContext = context;
        }

        // GET: /Products/MyWork
        // This action returns a view displaying a list of all products.
        public IActionResult MyWork()
        {
            try
            {
                // Fetch all products from the database.
                List<Products> products = identityContext.Products.ToList();
                Console.WriteLine("Number of products retrieved: " + products.Count); // Add this line for debugging
                return View(products);
            }
            catch (Exception ex)
            {
                // Log any exceptions and return an empty view.
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        // GET: /Products/Cart
        // This action returns a view displaying the current user's cart items.
        // Only accessible to authenticated users.
        [Authorize]
        public IActionResult Cart()
        {
            // Get the current user's ID.
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // Fetch the cart items for the current user, including related product data.
            var cartItems = identityContext.CartOrders
                                           .Include(c => c.Product)
                                           .Where(c => c.UserId == userId)
                                           .ToList();
            return View(cartItems);
        }

        // POST: /Products/AddToCart
        // This action handles the addition of a product to the cart.
        // Only accessible to authenticated users.
        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(int productId)
        {
            // Find the product with the specified ID.
            var product = identityContext.Products.FirstOrDefault(p => p.Product_ID == productId);
            if (product == null)
            {
                return NotFound();
            }

            // Check if the product is available.
            if (product.Quantity <= 0)
            {
                TempData["ErrorMessage"] = "Product is not available.";
                return RedirectToAction(nameof(MyWork));
            }

            // Get the current user's ID.
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // Create a new cart order for the product.
            var cartOrder = new CartOrders
            {
                Product_ID = productId,
                UserId = userId
            };

            // Add the cart order to the database and update the product quantity and availability.
            identityContext.CartOrders.Add(cartOrder);
            product.Quantity--;
            product.Availability = product.Quantity > 0;

            try
            {
                // Save changes to the database.
                identityContext.SaveChanges();
                TempData["SuccessMessage"] = "Product added to cart successfully.";
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the save operation.
                TempData["ErrorMessage"] = $"An error occurred while adding the product to the cart: {ex.Message}";
            }

            return RedirectToAction(nameof(Cart));
        }

        // POST: /Products/RemoveFromCart
        // This action handles the removal of a product from the cart.
        // Only accessible to authenticated users.
        [HttpPost]
        [Authorize]
        public IActionResult RemoveFromCart(int cartOrderId)
        {
            // Find the cart order with the specified ID, including related product data.
            var cartOrder = identityContext.CartOrders.Include(c => c.Product).FirstOrDefault(c => c.Cart_ID == cartOrderId);
            if (cartOrder == null)
            {
                return NotFound();
            }

            // Remove the cart order from the database and update the product quantity and availability.
            identityContext.CartOrders.Remove(cartOrder);
            cartOrder.Product.Quantity++;
            cartOrder.Product.Availability = true;

            try
            {
                // Save changes to the database.
                identityContext.SaveChanges();
                TempData["SuccessMessage"] = "Product removed from cart successfully.";
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the save operation.
                TempData["ErrorMessage"] = $"An error occurred while removing the product from the cart: {ex.Message}";
            }

            return RedirectToAction(nameof(Cart));
        }

        // POST: /Products/Pay
        // This action handles the payment process for the items in the cart.
        // Only accessible to authenticated users.
        [HttpPost]
        [Authorize]
        public IActionResult Pay()
        {
            // Get the current user's ID.
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // Fetch the cart items for the current user, including related product data.
            var cartItems = identityContext.CartOrders
                                           .Include(c => c.Product)
                                           .Where(c => c.UserId == userId)
                                           .ToList();

            // Create a sales statement for each item in the cart and remove the item from the cart.
            foreach (var item in cartItems)
            {
                var salesStatement = new SalesStatement
                {
                    Product_ID = item.Product_ID,
                    UserId = userId,
                    Purchase_Date = DateTime.Now,
                    Status = "Pending"
                };

                identityContext.SalesStatement.Add(salesStatement);
                identityContext.CartOrders.Remove(item);
            }

            // Save changes to the database.
            identityContext.SaveChanges();

            // Set a temporary message indicating the order has been placed.
            TempData["OrderPlacedMessage"] = "Payment pending. Order placed successfully.";
            return RedirectToAction(nameof(Cart));
        }

        // GET: /Products/History
        // This action returns a view displaying the purchase history of the current user.
        // Only accessible to authenticated users.
        [Authorize]
        public IActionResult History()
        {
            // Get the current user's ID.
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // Fetch the sales statements for the current user, including related product data.
            var transactions = identityContext.SalesStatement
                                              .Where(s => s.UserId == userId)
                                              .Include(s => s.Product)
                                              .ToList();
            return View("AccessControl/History", transactions);
        }
    }
}
