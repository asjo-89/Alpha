//using Alpha_Mvc.Models;
//using Alpha_Mvc.Models.ViewModels;
//using AspNetCoreGeneratedDocument;
//using Business.Dtos;
//using Business.Factories;
//using Business.Interfaces;
//using Business.Models;
//using Data.Entities;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace Alpha_Mvc.Controllers
//{
//    public class AdminController(IWebHostEnvironment environment, IMemberService memberService) : Controller
//    {
//        private readonly IWebHostEnvironment _environment = environment;
//        private readonly IMemberService _memberService = memberService;

//        public CreateProjectFormModel createProjectFormModel = new();
//        public CreateMemberFormModel createMemberFormModel = new();

//        public TeamMembersViewModel teamMembersViewModel = new();

//        public async Task<IActionResult> Index()
//        {
//            ViewData["Title"] = "Admin";
//            ViewData["Header"] = "Team Members";

//            var members = await _memberService.GetAllMembersAsync();

//            var viewModel = new TeamMembersViewModel
//            {
//                Users = members.Select(member => new UserModel
//                {
//                    Id = member.Id,
//                    FirstName = member.FirstName,
//                    LastName = member.LastName,
//                    Email = member.Email,
//                    PhoneNumber = member.PhoneNumber,
//                    JobTitle = member.JobTitle,
//                    ProfilePicture = Url.Content($"~/{member.ProfileImage}")
//                }),
//                Member = new CreateMemberFormModel()
//            };
//            return View(viewModel);
//        }


//        [HttpPost]
//        public async Task<IActionResult> AddMember(CreateMemberFormModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                var members = await _memberService.GetAllMembersAsync();
//                var viewModel = new TeamMembersViewModel
//                {
//                    Member = model,
//                    Users = members.Select(member => new UserModel
//                    {
//                        Id = member.Id,
//                        FirstName = member.FirstName,
//                        LastName = member.LastName,
//                        Email = member.Email,
//                        PhoneNumber = member.PhoneNumber,
//                        JobTitle = member.JobTitle
//                    }),
//                };
//                return View("Index", viewModel);
//            }

//            MemberUserEntity entity = new()
//            {
//                FirstName = model.FirstName,
//                LastName = model.LastName,
//                Email = model.Email,
//                Password = ""
//            };

//            var existingEntity = await _memberService.ExistsAsync(x => x.Email == entity.Email);
//            if (existingEntity)
//            {
//                // returnera att användaren redan finns.
//            }

//            var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");
//            Directory.CreateDirectory(directoryPath);

//            var fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
//            var filePath = Path.Combine(directoryPath, fileName);
//            var relativePath = $"uploads/{fileName}";

//            using (var fileStream = new FileStream(filePath, FileMode.Create))
//            {                
//                await model.ProfileImage.CopyToAsync(fileStream);
//            }

//            CreateMemberRegForm dto = new CreateMemberRegForm()
//            {
//                FirstName = model.FirstName,
//                LastName = model.LastName,
//                Email = model.Email,
//                PhoneNumber = model.PhoneNumber,
//                JobTitle = model.JobTitle,
//                StreetAddress = model.StreetAddress,
//                PostalCode = model.PostalCode,
//                City = model.City,
//                PassWord = "AddMember123!",
//                DateOfBirth = new (model.BirthYear, model.BirthMonth, model.BirthDay),
//                ProfileImage = relativePath,
//            };

//            var newModel = await _memberService.AddMemberAsync(dto);
//            if (newModel != null) 
//            {
//                return RedirectToAction("Index");
//            }
//            else
//            {
//                var members = await _memberService.GetAllMembersAsync();
//                var viewModel = new TeamMembersViewModel
//                {
//                    Member = model,
//                    Users = members.Select(member => new UserModel
//                    {
//                        Id = member.Id,
//                        FirstName = member.FirstName,
//                        LastName = member.LastName,
//                        Email = member.Email,
//                        PhoneNumber = member.PhoneNumber,
//                        JobTitle = member.JobTitle,
//                        ProfilePicture = Url.Content($"~/{member.ProfileImage}")
//                    }),
//                };

//                ModelState.AddModelError("viewModel", "Failed to create member.");
//                return View("Index", viewModel);
//            }
            
//        }


//        [HttpPost]
//        public async Task<IActionResult> EditMember(UserModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                var members = await _memberService.GetAllMembersAsync();
//                var viewModel = new TeamMembersViewModel
//                {
//                    Member = new CreateMemberFormModel(),
//                    Users = members.Select(member => new UserModel
//                    {
//                        Id = member.Id,
//                        FirstName = member.FirstName,
//                        LastName = member.LastName,
//                        Email = member.Email,
//                        PhoneNumber = member.PhoneNumber,
//                        JobTitle = member.JobTitle
//                    }),
//                };
//                return View("Index", viewModel);
//            }

//            var existingMember = await _memberService.GetMemberAsync(x => x.Id == model.Id);
//            if (existingMember == null) return null!;

//            string? relativePath = null;

//            if (model.ProfilePicture != null)
//            {
//                var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");
//                Directory.CreateDirectory(directoryPath);

//                var fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
//                var filePath = Path.Combine(directoryPath, fileName);
//                relativePath = $"uploads/{fileName}";

//                using (var fileStream = new FileStream(filePath, FileMode.Create))
//                {
//                    await model.ProfileImage.CopyToAsync(fileStream);
//                };
//            }

//            var memberModel = new MemberModel()
//            {
//                Id = model.Id,
//                FirstName = model.FirstName,
//                LastName = model.LastName,
//                Email = model.Email,
//                PhoneNumber = model.PhoneNumber,
//                JobTitle = model.JobTitle,
//                StreetAddress = model.StreetAddress,
//                PostalCode = model.PostalCode,
//                City = model.City,
//                DateOfBirth = new(model.BirthYear, model.BirthMonth, model.BirthDay),
//                ProfileImage = relativePath ?? existingMember.ProfileImage
//            };

//            MemberModel updatedModel = await _memberService.UpdateMember(memberModel);
//            if (updatedModel == null) return null!;

//            return RedirectToAction("Index");
            
//        }


//        [HttpPost]
//        public async Task<IActionResult> DeleteMember(Guid id)
//        {
//            var result = await _memberService.DeleteMember(id);

//            if (result)
//            {
//                return RedirectToAction("Index");
//            }
//            else
//            {
//                return View("Index");
//            }
//        }


//        [Route("add-project")]
//        [HttpPost]
//        public IActionResult AddProject()
//        {
//            ViewData["Title"] = "Admin";
//            return View(createProjectFormModel);
//        }        
//    }
//}