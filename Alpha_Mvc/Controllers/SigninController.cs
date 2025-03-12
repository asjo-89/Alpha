using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers
{
    public class SigninController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Sign In";
            return View();
        }

        [Route("create")]
        public IActionResult CreateAccount()
        {
            ViewData["Title"] = "Sign In";
            return View();
        }
    }
}
