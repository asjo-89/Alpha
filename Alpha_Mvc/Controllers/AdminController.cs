using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Title"] = "Admin";
            return View();
        }
    }
}