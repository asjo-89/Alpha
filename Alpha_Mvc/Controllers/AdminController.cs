using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers
{
    [Route("/admin")]
    public class AdminController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Admin";
            ViewData["Header"] = "Team Members";
            return View();
        }
    }
}