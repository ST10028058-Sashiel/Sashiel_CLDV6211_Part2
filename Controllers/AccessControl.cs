using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sashiel_CLDV6211_Part2.Areas.Identity.Data;
using Sashiel_CLDV6211_Part2.Models;
using Microsoft.AspNetCore.Identity;

namespace Sashiel_CLDV6211_Part2.Controllers
{
    // The AccessControlController handles role management and access to sales statements.
    // It uses ASP.NET Core Identity for role management and Entity Framework Core for data access.
    public class AccessControlController : Controller
    {
        // Private fields to hold the RoleManager and IdentityContext instances.
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IdentityContext identityContext;

        // Constructor to inject the RoleManager and IdentityContext instances via dependency injection.
        public AccessControlController(RoleManager<IdentityRole> roleManager, IdentityContext context)
        {
            this.roleManager = roleManager;
            identityContext = context;
        }

        // GET: /AccessControl/Roles
        // This action returns a view displaying a list of roles.
        public IActionResult Roles()
        {
            // Fetch all roles from the RoleManager.
            List<IdentityRole> roles = roleManager.Roles.ToList();
            return View(roles);
        }

        // GET: /AccessControl/RoleCreate
        // This action returns a view for creating a new role.
        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }

        // POST: /AccessControl/RoleCreate
        // This action handles the POST request to create a new role.
        [HttpPost]
        public async Task<IActionResult> RoleCreate(IdentityRole model)
        {
            // Check if the role does not already exist, then create it.
            if (!await roleManager.RoleExistsAsync(model.Name))
            {
                await roleManager.CreateAsync(new IdentityRole(model.Name));
            }
            return RedirectToAction("Roles");
        }

        // GET: /AccessControl/AdminHistory
        // This action returns a view displaying sales statements for the admin.
        // Only accessible to users in the "Admin" role.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminHistory()
        {
            // Fetch all sales statements from the database, including related product and user data.
            List<ProductStatement> salesStatements = await identityContext.ProductStatement
                                                .Include(s => s.Product)
                                                .Include(s => s.User)
                                                .ToListAsync();
            return View(salesStatements);
        }

        // GET: /AccessControl/History
        // This action returns a view displaying the sales history of the current user.
        // Only accessible to authenticated users.
        [Authorize]
        public async Task<IActionResult> History()
        {
            // Get the current user's ID.
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // Fetch the sales statements for the current user, including related product data.
            List<ProductStatement> salesStatements = await identityContext.ProductStatement
                                                .Where(s => s.UserId == userId)
                                                .Include(s => s.Product)
                                                .ToListAsync();
            return View(salesStatements);
        }

        // POST: /AccessControl/ApproveTransaction
        // This action handles the POST request to approve a transaction.
        // Only accessible to users in the "Admin" role.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ApproveTransaction(int statementId)
        {
            // Find the sales statement with the specified ID.
            ProductStatement salesStatement = await identityContext.ProductStatement.FindAsync(statementId);
            if (salesStatement != null)
            {
                // Set the status of the sales statement to "Approved" and save changes.
                salesStatement.Status = "Approved";
                await identityContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AdminHistory));
        }
    }
}
