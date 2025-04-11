using Alpha_Mvc.Models;
using Alpha_Mvc.Models.ViewModels;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ProjectsViewModel projectsViewModel = new();
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            ViewData["Header"] = "Projects";

            ProjectsViewModel viewModel = new()
            {
                ProjectForm = new ProjectFormModel(),
                ProjectModel = new Project(),
                ClientModel = new ClientModel(),
                StatusModel = new StatusModel(),
            };

            return View(viewModel);
        }
    }
}
