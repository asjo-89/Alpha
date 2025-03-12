using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers
{
    [Route("projects")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            ViewData["Header"] = "Projects";
            return View();
        }
    }
}
