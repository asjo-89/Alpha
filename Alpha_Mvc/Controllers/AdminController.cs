using Alpha_Mvc.Factories;
using Alpha_Mvc.Interfaces;
using Alpha_Mvc.Models;
using Alpha_Mvc.ViewModels;
using Business.Interfaces;
using Data.Entities;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Mvc.Controllers
{
    public class AdminController(IWebHostEnvironment environment, RoleManager<IdentityRole<Guid>> roleManager, UserManager<MemberUserEntity> userManager, IMemberUserService memberService, IAddressService addressService, IPictureService pictureService, IFileService fileService) : Controller
    {
        private readonly IWebHostEnvironment _environment = environment;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;
        private readonly UserManager<MemberUserEntity> _userManager = userManager;

        private readonly IFileService _fileService = fileService;
        private readonly IMemberUserService _memberService = memberService;
        private readonly IAddressService _addressService = addressService;
        private readonly IPictureService _pictureService = pictureService;

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
                    ImageUrl = Url.Content($"{member.ImageUrl}"),
                    StreetAddress = member.Address?.StreetAddress ?? "",
                    PostalCode = member.Address?.PostalCode ?? "",
                    City = member.Address?.City ?? "",
                    BirthDay = member.DateOfBirth?.Day ?? 0,
                    BirthMonth = member.DateOfBirth?.Month ?? 0,
                    BirthYear = member.DateOfBirth?.Year ?? 0,
                    PictureId = member.PictureId
                }),
                Member = new MemberFormModel(),
                Roles = roles.Select(role => new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name,
                }).ToList()
            };

            ViewBag.Roles = viewModel.Roles;


            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddMember([Bind(Prefix = "Member")] MemberFormModel model)
        {
            Console.WriteLine("AddMember");
            if (model.BirthDay > 0 && model.BirthMonth > 0 && model.BirthYear > 0)
            {
                model.DateOfBirth = new DateOnly(model.BirthYear, model.BirthMonth, model.BirthDay);
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray());

                foreach (var error in errors)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value)}");
                }
                return BadRequest(new { success = false, errors });
            }

            var relativePath = await _fileService.CreateFile(model.ProfileImage);

            var picture = await _pictureService.CreateAsync(relativePath);
            if (!picture.Succeeded || picture.Data == null)
                return BadRequest();

            var dto = MemberUserFactoryMCV.CreateDtoFromModel(model);
            dto.PictureId = picture.Data.Id;

            var result = await _memberService.CreateAsync(dto);
            var newMember = await _memberService.GetMemberUserAsync(dto.Email);
            if (!newMember.Succeeded || newMember.Data == null)
                return BadRequest($"An error occured creating the new image.\n{picture.StatusCode}\n{picture.ErrorMessage}");

            var address = await _addressService.CreateAsync(model.StreetAddress, model.PostalCode, model.City, newMember.Data.Id);
           
            if (!result.Succeeded || !address.Succeeded)
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

        //[HttpGet]
        //public async Task<IActionResult> EditMemberModal(Guid id)
        //{
        //    var memberResult = await _memberService.GetMemberUserAsync(id);
        //    if (!memberResult.Succeeded || memberResult.Data == null)
        //        return NotFound("Member was not found.");

        //    var roles = await _roleManager.Roles.ToListAsync();
        //    var memberUser = await _userManager.FindByIdAsync(id.ToString());
        //    if (memberUser == null)
        //        return NotFound("No member was found.");

        //    var memberRoles = await _userManager.GetRolesAsync(memberUser) ?? new List<string>();
        //    if (memberRoles == null)
        //        return NotFound($"No role found for member with with id {id} was found.");
            
        //    var role = memberRoles.FirstOrDefault() ?? "";

        //    var editViewModel = new EditMemberViewModel
        //    {
        //        Member = new MemberUserModel
        //        {
        //            Id = memberResult.Data.Id,
        //            FirstName = memberResult.Data.FirstName,
        //            LastName = memberResult.Data.LastName,
        //            Email = memberResult.Data.Email,
        //            PhoneNumber = memberResult.Data.PhoneNumber ?? "",
        //            JobTitle = memberResult.Data.JobTitle ?? "",
        //            StreetAddress = memberResult.Data.Address?.StreetAddress ?? "",
        //            PostalCode = memberResult.Data.Address?.PostalCode ?? "",
        //            City = memberResult.Data.Address?.City ?? "",
        //            BirthDay = memberResult.Data.DateOfBirth?.Day,
        //            BirthMonth = memberResult.Data.DateOfBirth?.Month,
        //            BirthYear = memberResult.Data.DateOfBirth?.Year,
        //            ImageUrl = memberResult.Data.ImageUrl
        //        },
        //        Roles = roles.Select(role => new SelectListItem
        //        {
        //            Value = role.Id.ToString(),
        //            Text = role.Name
        //        }).ToList()

        //    };

        //    return PartialView("Sections/_EditMemberSection", editViewModel); 
        //}

        [HttpPost]
        public async Task<IActionResult> EditMember(MemberUserModel model, string? existingUrl)
        {
            if (model.BirthDay > 0 && model.BirthMonth > 0 && model.BirthYear > 0)
            {
                model.DateOfBirth = new DateOnly(model.BirthYear.Value, model.BirthMonth.Value, model.BirthDay.Value);
            }

            if (!ModelState.IsValid)
            {
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
                        ImageUrl = Url.Content($"{member.ImageUrl}"),
                        StreetAddress = member.Address?.StreetAddress ?? "",
                        PostalCode = member.Address?.PostalCode ?? "",
                        City = member.Address?.City ?? "",
                        BirthDay = member.DateOfBirth?.Day ?? 0,
                        BirthMonth = member.DateOfBirth?.Month ?? 0,
                        BirthYear = member.DateOfBirth?.Year ?? 0
                    }),
                    Member = new MemberFormModel(),
                    Roles = roles.Select(role => new SelectListItem
                    {
                        Value = role.Id.ToString(),
                        Text = role.Name,
                    }).ToList()
                };

                ViewBag.Roles = viewModel.Roles;

                ModelState.AddModelError("viewModel", "Failed to update member.");
                return PartialView("Sections/_EditMemberSection", viewModel);
            }

            string relativePath = "";
            var memberDto = new MemberUserDto();

            if (model.ProfileImage == null)
            {
                relativePath = existingUrl;
                model.ImageUrl = Url.Content(relativePath);
                memberDto = MemberUserFactoryMCV.CreateDtoFromModel(model);
                memberDto.PictureId = model.PictureId;
            }
            else
            {
                relativePath = await _fileService.CreateFile(model.ProfileImage);
                var picture = await _pictureService.CreateAsync(relativePath);
                if (!picture.Succeeded || picture.Data == null)
                    return BadRequest($"An error occured creating the new image.\n{picture.StatusCode}\n{picture.ErrorMessage}");

                model.ImageUrl = Url.Content(relativePath);
                memberDto = MemberUserFactoryMCV.CreateDtoFromModel(model);
                memberDto.PictureId = picture.Data.Id;                
            }
            var result = await _memberService.UpdateAsync(memberDto);

            var newMember = await _memberService.GetMemberUserAsync(memberDto.Email);
            if (!newMember.Succeeded || newMember.Data == null)
                return BadRequest($"An error occured updating member.");

            //if (relativePath == model.ImageUrl)
            //{
            //    _fileService.DeleteFile(relativePath);
            //    var dto = MemberUserFactoryMCV.CreateDtoFromModel(model);

            //    var updatedModel = await _memberService.UpdateAsync(dto);
            //    if (!updatedModel.Succeeded) 
            //        return BadRequest($"An error occured updating the member.\n{updatedModel.StatusCode}\n{updatedModel.ErrorMessage}");

            //    return RedirectToAction("Index");
            //}



            var address = await _addressService.CreateAsync(model.StreetAddress, model.PostalCode, model.City, newMember.Data.Id);

            if (!result.Succeeded || !address.Succeeded)
            {
                return BadRequest($"An error occured updating the member.\n{result.StatusCode}\n{result.ErrorMessage}");
            }
            else
            {
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
                        ImageUrl = Url.Content($"{member.ImageUrl}"),
                        StreetAddress = member.Address?.StreetAddress ?? "",
                        PostalCode = member.Address?.PostalCode ?? "",
                        City = member.Address?.City ?? "",
                        BirthDay = member.DateOfBirth?.Day ?? 0,
                        BirthMonth = member.DateOfBirth?.Month ?? 0,
                        BirthYear = member.DateOfBirth?.Year ?? 0
                    }),
                    Member = new MemberFormModel(),
                    Roles = roles.Select(role => new SelectListItem
                    {
                        Value = role.Id.ToString(),
                        Text = role.Name,
                    }).ToList()
                };

                ViewBag.Roles = viewModel.Roles;
                ModelState.AddModelError("viewModel", "Failed to create member.");
                return View("Index", viewModel);
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var memberUser = await _memberService.GetMemberUserAsync(id);

            if (!memberUser.Succeeded || memberUser.Data == null || memberUser.Data.ImageUrl == null)
                return NotFound($"Status code: {memberUser.StatusCode}\nError message: \n{memberUser.ErrorMessage}");

            var filePath = Path.Combine(_environment.WebRootPath, memberUser.Data.ImageUrl.TrimStart('/'));
            var result = await _memberService.DeleteAsync(id);

            if (result.Succeeded)
            {                
                var deleteFile = _fileService.DeleteFile(filePath);
                
                return deleteFile
                    ? RedirectToAction("Index")
                    : BadRequest();
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
    }
}