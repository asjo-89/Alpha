using Alpha_Mvc.Factories;
using Alpha_Mvc.Models;
using Alpha_Mvc.ViewModels;
using Business.Interfaces;
using Data.Entities;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Mvc.Controllers
{
    public class AdminController(IWebHostEnvironment environment, RoleManager<IdentityRole<Guid>> roleManager, IMemberUserService memberService, IAddressService addressService, IPictureService pictureService) : Controller
    {
        private readonly IWebHostEnvironment _environment = environment;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;

        private readonly IMemberUserService _memberService = memberService;
        private readonly IAddressService _addressService = addressService;
        private readonly IPictureService _pictureService = pictureService;

        public CreateProjectFormModel createProjectFormModel = new();
        public MemberFormModel createMemberFormModel = new();

        public TeamMembersViewModel teamMembersViewModel = new();

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Admin";
            ViewData["Header"] = "Team Members";

            var members = await _memberService.GetMemberUsersAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new TeamMembersViewModel
            {
                Users = members.Data.Select(member => new MemberUserModel
                {
                    Id = member.Id,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    PhoneNumber = member.PhoneNumber ?? "",
                    JobTitle = member.JobTitle ?? "No role assigned",
                    ImageUrl = Url.Content($"{member.ImageUrl}")
                }),
                Member = new MemberFormModel(),
                Roles = roles.Select(role => new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name,
                }).ToList()
            };
            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddMember([Bind(Prefix = "Member")]MemberFormModel model)
        {
            if (model.BirthDay > 0 && model.BirthMonth > 0 && model.BirthYear > 0)
            {
                model.DateOfBirth = new DateOnly(model.BirthYear, model.BirthMonth, model.BirthDay);
            }
            
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                     );
                foreach (var error in errors)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value)}");
                }
                return BadRequest(new { success = false, errors });
            }
            
            // * L�gg ut i egen metod
            var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(directoryPath);

            var fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
            var filePath = Path.Combine(directoryPath, fileName);
            var relativePath = $"uploads/{fileName}";

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProfileImage.CopyToAsync(fileStream);
            }
            // *

            model.ImageUrl = relativePath;       
            
            var picture = await _pictureService.CreateAsync(model.ImageUrl);
            if (!picture.Succeeded)
                return BadRequest();

            var address = await _addressService.CreateAsync(model.StreetAddress, model.PostalCode, model.City);
            if (!address.Succeeded)
                return BadRequest();

            var dto = MemberUserFactoryMCV.CreateDtoFromModel(model);
            dto.PictureId = picture.Data?.Id;
            dto.AddressId = address.Data?.Id;

            var result = await _memberService.CreateAsync(dto);
            if (result.Succeeded)
            {
                return BadRequest();
            }
            else
            {
                var members = await _memberService.GetMemberUsersAsync();

                var viewModel = new TeamMembersViewModel
                {
                    Users = members.Data.Select(member => new MemberUserModel
                    {
                        Id = member.Id,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        Email = member.Email,
                        PhoneNumber = member.PhoneNumber ?? "",
                        JobTitle = member.JobTitle ?? "No role assigned",
                        ImageUrl = Url.Content($"{member.ImageUrl}")
                    }),
                    Member = new MemberFormModel()
                };

                ModelState.AddModelError("viewModel", "Failed to create member.");
                return View("Index", viewModel);
            }

        }


        [HttpPost]
        public async Task<IActionResult> EditMember(MemberUserModel model)
        {
            if (!ModelState.IsValid)
            {
                var members = await _memberService.GetMemberUsersAsync();

                var viewModel = new TeamMembersViewModel
                {
                    Users = members.Data.Select(member => new MemberUserModel
                    {
                        Id = member.Id,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        Email = member.Email,
                        PhoneNumber = member.PhoneNumber ?? "",
                        JobTitle = member.JobTitle ?? "No role assigned",
                        ImageUrl = Url.Content($"{member.ImageUrl}")
                    }),
                    Member = new MemberFormModel()
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
            }
            ;

            MemberUserDto memberModel = new()
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
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var memberUser = await _memberService.GetMemberUserAsync(id);

            var result = await _memberService.DeleteAsync(memberUser.Data);

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