using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            ViewData["Header"] = "Projects";
            return View();
        }
    }
}
