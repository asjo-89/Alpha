using Alpha_Mvc.Models;
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

        public IActionResult SignIn(SignInFormModel data)
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

        [Route("add-project")]
        public IActionResult AddProject()
        {
            ViewData["Title"] = "Add Project";
            return View();
        }

        [Route("add-member")]
        public IActionResult AddMember()
        {
            ViewData["Title"] = "Add Project";
            return View();
        }
    }
}
