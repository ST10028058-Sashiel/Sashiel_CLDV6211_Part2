using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Sashiel_CLDV6211_Part2.Areas.Identity.Data;
using Sashiel_CLDV6211_Part2.Models;
using Microsoft.AspNetCore.Identity;

namespace Sashiel_CLDV6211_Part2.Controllers
{
    public class AccessControlController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IdentityContext _context;

        public AccessControlController(RoleManager<IdentityRole> roleManager, IdentityContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        // Role Management Actions
        public IActionResult Roles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(IdentityRole model)
        {
            if (!await _roleManager.RoleExistsAsync(model.Name))
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Name));
            }
            return RedirectToAction("Roles");
        }

        // Transaction Management Actions

        // Admin view for managing all user purchases and approving pending orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminHistory()
        {
            var salesStatements = await _context.SalesStatement
                                                .Include(s => s.Product)
                                                .Include(s => s.User)
                                                .ToListAsync();
            return View(salesStatements);
        }

        // Regular user view for their purchase history
        [Authorize]
        public async Task<IActionResult> History()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var salesStatements = await _context.SalesStatement
                                                .Where(s => s.UserId == userId)
                                                .Include(s => s.Product)
                                                .ToListAsync();
            return View(salesStatements);
        }

        // Approve a pending transaction
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ApproveTransaction(int statementId)
        {
            var salesStatement = await _context.SalesStatement.FindAsync(statementId);
            if (salesStatement != null)
            {
                salesStatement.Status = "Approved";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AdminHistory));
        }
    }
}
