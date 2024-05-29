using Microsoft.AspNetCore.Mvc;
using Sashiel_CLDV6211_Part2.Models;
using System.Diagnostics;

namespace Sashiel_CLDV6211_Part2.Controllers
{
 
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;

      
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /Home/Index
        // This action returns the main homepage view.
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Privacy
        // This action returns the privacy policy view.
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/ContactUs
        // This action returns the contact us view.
        public IActionResult ContactUs()
        {
            return View();
        }

        // GET: /Home/AboutUs
        // This action returns the about us view.
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
