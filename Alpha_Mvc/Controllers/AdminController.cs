using Alpha_Mvc.Models;
using AspNetCoreGeneratedDocument;
using Business.Dtos;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers
{
    public class AdminController(IWebHostEnvironment environment, ICreateMemberService memberService) : Controller
    {
        private readonly IWebHostEnvironment _environment = environment;
        private readonly ICreateMemberService _memberService = memberService;

        public CreateProjectFormModel createProjectFormModel = new();
        public CreateMemberFormModel createMemberFormModel = new();

        public IActionResult Index()
        {
            ViewData["Title"] = "Admin";
            ViewData["Header"] = "Team Members";
            return View();
        }

        [HttpPost]
        public IActionResult Index(CreateMemberFormModel model)
        {
            ViewData["Title"] = "Admin";

            if (!ModelState.IsValid)
                return View();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(CreateMemberFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            if (model.ProfileImage.Length == 0)
            {
                ModelState.AddModelError("ProfileImage", "Profile image is required.");
                return View("Index", model);
            }

            var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(directoryPath);

            var filePath = Path.Combine(directoryPath, $"{Guid.NewGuid()}_{model.ProfileImage.FileName}");

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStream);
            }

            CreateMemberRegForm dto = new CreateMemberRegForm()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.Email,
                PhoneNumber = model.Phone,
                StreetAddress = model.StreetAddress,
                PostalCode = model.PostalCode,
                Role = model.Role,
                City = model.City,
                DateOfBirth = model.DateOfBirth,
                ProfileImage = filePath
            };

            var newModel = await _memberService.AddMember(dto);
            if (newModel != null) 
            {
                return View("Index", model);
            }
            else
            {
                return View("Index", model);
            }
            
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