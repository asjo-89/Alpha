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

        [Route("add")]
        public IActionResult AddProject()
        {
            ViewData["Title"] = "Add Project";
            return View();
        }
    }
}
