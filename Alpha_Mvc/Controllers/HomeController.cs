﻿using Alpha_Mvc.Models;
using Alpha_Mvc.Models.ViewModels;
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

            ProjectsViewModel viewModel = new ProjectsViewModel()
            {
                Project = new ProjectFormModel(),
                ProjectModel = new ProjectModel()
            };

            return View(viewModel);
        }
    }
}
