using Alpha_Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers
{
    [Route("/admin")]
    public class AdminController : Controller
    {
        public CreateProjectFormModel createProjectFormModel = new();
        public CreateMemberFormModel createMemberFormModel = new();

        [Route("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Admin";
            ViewData["Header"] = "Team Members";
            return View();
        }

        [Route("")]
        [HttpPost]
        public IActionResult Index(CreateMemberFormModel model)
        {
            ViewData["Title"] = "Admin";

            if (!ModelState.IsValid)
                return View();

            return View();
        }

        [Route("add-project")]
        [HttpPost]
        public IActionResult AddProject()
        {
            ViewData["Title"] = "Admin";
            return View(createProjectFormModel);
        }        
    }
}