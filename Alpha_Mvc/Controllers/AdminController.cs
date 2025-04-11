using Alpha_Mvc.Models;
using Alpha_Mvc.Models.ViewModels;
using Business.Interfaces;
using Data.Entities;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers
{
    public class AdminController(IWebHostEnvironment environment, IMemberUserService memberService) : Controller
    {
        private readonly IWebHostEnvironment _environment = environment;
        private readonly IMemberUserService _memberService = memberService;

        public CreateProjectFormModel createProjectFormModel = new();
        public CreateMemberFormModel createMemberFormModel = new();

        public TeamMembersViewModel teamMembersViewModel = new();

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Admin";
            ViewData["Header"] = "Team Members";

            var members = await _memberService.GetMemberUsersAsync();

            var viewModel = new TeamMembersViewModel
            {
                Users = members.Data.Select(member => new UserModel
                {
                    Id = member.Id,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    PhoneNumber = member.PhoneNumber ?? "",
                    JobTitle = member.JobTitle ?? "No role assigned",
                    ImageUrl = Url.Content($"{member.ImageUrl}")
                }),
                Member = new CreateMemberFormModel()
            };
            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddMember(CreateMemberFormModel model)
        {
            if (!ModelState.IsValid)
            {
                var members = await _memberService.GetMemberUsersAsync();

                var viewModel = new TeamMembersViewModel
                {
                    Users = members.Data.Select(member => new UserModel
                    {
                        Id = member.Id,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        Email = member.Email,
                        PhoneNumber = member.PhoneNumber ?? "",
                        JobTitle = member.JobTitle ?? "No role assigned",
                        ImageUrl = Url.Content($"{member.ImageUrl}")
                    }),
                    Member = new CreateMemberFormModel()
                };



                return RedirectToAction("AddMember");
            }

            MemberUserEntity entity = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = ""
            };

            var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(directoryPath);

            var fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
            var filePath = Path.Combine(directoryPath, fileName);
            var relativePath = $"uploads/{fileName}";

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProfileImage.CopyToAsync(fileStream);
            }

            MemberUserFormData dto = new MemberUserFormData()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                JobTitle = model.JobTitle,
                StreetAddress = model.StreetAddress,
                PostalCode = model.PostalCode,
                City = model.City,
                DateOfBirth = new(model.BirthYear, model.BirthMonth, model.BirthDay),
                ImageUrl = relativePath,
            };

            var newModel = await _memberService.CreateAsync(dto);
            if (newModel != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var members = await _memberService.GetMemberUsersAsync();

                var viewModel = new TeamMembersViewModel
                {
                    Users = members.Data.Select(member => new UserModel
                    {
                        Id = member.Id,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        Email = member.Email,
                        PhoneNumber = member.PhoneNumber ?? "",
                        JobTitle = member.JobTitle ?? "No role assigned",
                        ImageUrl = Url.Content($"{member.ImageUrl}")
                    }),
                    Member = new CreateMemberFormModel()
                };

                ModelState.AddModelError("viewModel", "Failed to create member.");
                return View("Index", viewModel);
            }

        }


        [HttpPost]
        public async Task<IActionResult> EditMember(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                var members = await _memberService.GetMemberUsersAsync();

                var viewModel = new TeamMembersViewModel
                {
                    Users = members.Data.Select(member => new UserModel
                    {
                        Id = member.Id,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        Email = member.Email,
                        PhoneNumber = member.PhoneNumber ?? "",
                        JobTitle = member.JobTitle ?? "No role assigned",
                        ImageUrl = Url.Content($"{member.ImageUrl}")
                    }),
                    Member = new CreateMemberFormModel()
                };
                return View("Index", viewModel);
            }

            var existingMember = await _memberService.ExistsAsync(model.Id);
            if (!existingMember.Data) return null!;

            string? relativePath = null;

            var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(directoryPath);

            var fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
            var filePath = Path.Combine(directoryPath, fileName);
            relativePath = $"uploads/{fileName}";

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProfileImage.CopyToAsync(fileStream);
            };

            MemberUserFormData memberModel = new()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                JobTitle = model.JobTitle,
                StreetAddress = model.StreetAddress,
                PostalCode = model.PostalCode,
                City = model.City,
                DateOfBirth = new(model.BirthYear, model.BirthMonth, model.BirthDay),
                ImageUrl = relativePath
            };

            var updatedModel = await _memberService.UpdateAsync(memberModel);
            if (!updatedModel.Data) return null!;

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteMember(UserModel model)
        {
            var formData = model.MapTo<MemberUserFormData>();

            var result = await _memberService.DeleteAsync(formData);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index");
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