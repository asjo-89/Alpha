using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers
{
    [Route("signin")]
    public class SigninController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Sign In";
            return View();
        }
    }
}
