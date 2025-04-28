using Alpha_Mvc.Models;
using Alpha_Mvc.ViewModels;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            ViewData["Header"] = "Dashboard";

            

            return View();
        }
    }
}
