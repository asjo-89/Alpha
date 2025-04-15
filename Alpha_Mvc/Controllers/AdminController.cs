using Alpha_Mvc.Models;
using Alpha_Mvc.ViewModels;
using Business.Interfaces;
using Data.Entities;
using Domain.Dtos;
using Domain.Extensions;
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
        public CreateMemberFormModel createMemberFormModel = new();

        public TeamMembersViewModel teamMembersViewModel = new();

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Admin";
            ViewData["Header"] = "Team Members";

            var members = await _memberService.GetMemberUsersAsync();
            var roles = await _roleManager.Roles.ToListAsync();

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
                Member = new CreateMemberFormModel(),
                Roles = roles.Select(role => new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name,
                }).ToList()
            };
            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddMember([Bind(Prefix = "Member")]CreateMemberFormModel model)
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
            
            var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(directoryPath);

            var fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
            var filePath = Path.Combine(directoryPath, fileName);
            var relativePath = $"uploads/{fileName}";

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProfileImage.CopyToAsync(fileStream);
            }
            
            model.ImageUrl = relativePath;            
            var dto = model.MapTo<MemberUserFormData>();

            var picture = await _pictureService.CreateAsync(model.ImageUrl);
            if (!picture.Succeeded)
                return BadRequest();
            dto.PictureId = picture.Data?.Id;            

            var newModel = await _memberService.CreateAsync(dto);
            if (newModel != null)
            {
                var newMember = await _memberService.GetMemberUserAsync(dto.Email);
                var address = await _addressService.CreateAsync(model.StreetAddress, model.PostalCode, model.City, newMember.Data.Id);
                if (!address.Succeeded)
                    return BadRequest();

                dto.AddressID = address.Data.Id;
                var test = await _memberService.UpdateAsync(dto);
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
            }
            ;

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