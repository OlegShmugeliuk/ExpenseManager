using ExpenseManager.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Security.Principal;

namespace ExpenseManager.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("/")]
		public IActionResult Index()
		{
            ViewData["UserId"] = _userManager.GetUserId(this.User);
            if (ViewData["UserId"] != null)
            {
                return RedirectToAction("Start", "Main");
            }
            else
            {
                Console.WriteLine(ViewData["UserId"]);

                return RedirectToPage("/Account/Register", new { area = "Identity" });
            }
            
            
		}
	}
}
