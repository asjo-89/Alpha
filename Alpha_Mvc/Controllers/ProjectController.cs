using Alpha_Mvc.Factories;
using Alpha_Mvc.Interfaces;
using Alpha_Mvc.Models;
using Alpha_Mvc.ViewModels;
using Business.Interfaces;
using Business.Models;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace Alpha_Mvc.Controllers
{
    public class ProjectController(IWebHostEnvironment environment, IFileService fileService, IProjectService projectService, IPictureService pictureService, IProjectMemberService pmService, IMemberUserService memberService, IClientService clientService) : Controller
    {
        private readonly IWebHostEnvironment _environment = environment;

        private readonly IFileService _fileService = fileService;
        private readonly IProjectService _projectService = projectService;
        private readonly IMemberUserService _memberService = memberService;
        private readonly IProjectMemberService _pmService = pmService;
        //private readonly IProjectNoteService _projectNoteService = projectNoteService;
        //private readonly IStatusService _statusService = statusService;
        private readonly IClientService _clientService = clientService;
        private readonly IPictureService _pictureService = pictureService;


        public ProjectsViewModel projectsViewModel = new();



        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Projects";
            ViewData["Header"] = "Add Projects";

            var members = await _memberService.GetMemberUsersAsync();
            var clients = await _clientService.GetClientsAsync();
            var cards = await _projectService.GetProjectCardsAsync();
            var projects = await _projectService.GetProjectsWithDetailsAsync();

            var viewModel = new ProjectsViewModel
            {
                Cards = cards.Data?.Select(ProjectFactoryMVC.CreateCardFromDomainModel).ToList() ?? [],
                AllMembers = members.Data?.Select(member => new MySelectListItem
                {
                    Value = member.Id.ToString(),
                    Text = $"{member.FirstName} {member.LastName}",
                    ImageUrl = member.ImageUrl
                }).ToList() ?? [],
                Clients = clients.Data?.Select(client => new SelectListItem
                {
                    Value = client.Id.ToString(),
                    Text = client.ClientName
                }).ToList() ?? []
            };

            ViewBag.AllMembers = viewModel.AllMembers;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditProject(Guid id)
        {
            var result = await _projectService.GetProjectAsync(id);
            Console.WriteLine($"#########################\n{result.Data}");
            if (!result.Succeeded || result.Data == null)
                return NotFound("Project was not found.");

            var members = await _pmService.GetProjectMembersWithDetailsAsync(result.Data.Id);

            var viewModel = new
            {
                Project = new
                {
                    result.Data.Id,
                    result.Data.ProjectTitle,
                    result.Data.Client.ClientName,
                    result.Data.Description,
                    result.Data.StartDate,
                    result.Data.EndDate,
                    result.Data.Budget,
                    result.Data.ImageUrl
                },
                Members = members.Select(member => new
                {
                    member.Id,
                    member.FirstName,
                    member.LastName,
                    member.ImageUrl
                })
            };

            return Json(viewModel);
        }





        [HttpPost]
        public async Task<IActionResult> AddProject(CreateProjectFormModel model, string? SelectedIds)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray());

                foreach (var error in errors)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value)}");
                }


                var membersList = await _memberService.GetMemberUsersAsync();
                var clientsList = await _clientService.GetClientsAsync();
                var cardsList = await _projectService.GetProjectCardsAsync();

                var viewModelAdd = new ProjectsViewModel
                {
                    Cards = cardsList.Data?.Select(ProjectFactoryMVC.CreateCardFromDomainModel).ToList() ?? [],
                    AllMembers = membersList.Data?.Select(member => new MySelectListItem
                    {
                        Value = member.Id.ToString(),
                        Text = $"{member.FirstName} {member.LastName}",
                        ImageUrl = member.ImageUrl
                    }).ToList() ?? [],
                    Clients = clientsList.Data?.Select(client => new SelectListItem
                    {
                        Value = client.Id.ToString(),
                        Text = client.ClientName
                    }).ToList() ?? []
                };

                ViewBag.ShowModalProject = true;
                return View("Index", viewModelAdd);
            }

            var relativePath = await _fileService.CreateFile(model.Picture);

            var picture = await _pictureService.CreateAsync(relativePath);
            if (!picture.Succeeded)
                return BadRequest("Picture could not be created.");

            var client = new ClientResult<Client>();
            var existingClient = await _clientService.GetClientAsync(model.ClientName);

            if (!existingClient.Succeeded)
            {
                client = await _clientService.CreateAsync(model.ClientName);
            }
            else if (!existingClient.Succeeded && !client.Succeeded)
            {
                return BadRequest("Client was not created.");
            }
            else
            {
                client = existingClient;
            }

            var dto = ProjectFactoryMVC.CreateDtoFromCreateForm(model, client.Data.Id);
            dto.PictureId = picture.Data.Id;

            if (existingClient.Data != null)
            {
                dto.ClientId = existingClient.Data?.Id;
            }
            else if (existingClient == null && client.Data != null)
            {
                dto.ClientId = client.Data?.Id;
            }

            var result = await _projectService.CreateAsync(dto);
            if (!result.Succeeded)
                return BadRequest("Project could not be created.");

            var existingMembers = await _pmService.ExistingAsync(dto);
            if (existingMembers.Any())
            {
                var remove = await _pmService.DeleteAsync(existingMembers);
            }

            if (!string.IsNullOrEmpty(SelectedIds))
            {
                //var memberIds = JsonSerializer.Deserialize<List<Guid>>(SelectedIds);
                var memberIds = SelectedIds
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(Guid.Parse)
                        .ToList();
                if (memberIds != null)
                {
                    foreach (var member in memberIds)
                    {
                        await _pmService.AddAsync(new ProjectMemberDto { ProjectId = result.Data.Id, MemberId = member });
                    }
                }
            }
            var members = await _memberService.GetMemberUsersAsync();
            var clients = await _clientService.GetClientsAsync();
            var cards = await _projectService.GetProjectCardsAsync();

            var viewModel = new ProjectsViewModel
            {
                Cards = cards.Data?.Select(ProjectFactoryMVC.CreateCardFromDomainModel).ToList() ?? [],
                AllMembers = members.Data?.Select(member => new MySelectListItem
                {
                    Value = member.Id.ToString(),
                    Text = $"{member.FirstName} {member.LastName}",
                    ImageUrl = member.ImageUrl
                }).ToList() ?? [],
                Clients = clients.Data?.Select(client => new SelectListItem
                {
                    Value = client.Id.ToString(),
                    Text = client.ClientName
                }).ToList() ?? []
            };

            return View("Index", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProject(EditProjectFormModel model, string? SelectedIds)
        {
            if (!ModelState.IsValid)
            {
                var memberList = await _memberService.GetMemberUsersAsync();
                var clientList = await _clientService.GetClientsAsync();
                var cardList = await _projectService.GetProjectCardsAsync();

                var editViewModel = new ProjectsViewModel
                {
                    Cards = cardList.Data?.Select(ProjectFactoryMVC.CreateCardFromDomainModel).ToList() ?? [],
                    AllMembers = memberList.Data?.Select(member => new MySelectListItem
                    {
                        Value = member.Id.ToString(),
                        Text = $"{member.FirstName} {member.LastName}",
                        ImageUrl = member.ImageUrl
                    }).ToList() ?? [],
                    Clients = clientList.Data?.Select(client => new SelectListItem
                    {
                        Value = client.Id.ToString(),
                        Text = client.ClientName
                    }).ToList() ?? []
                };
                return RedirectToAction("Index", editViewModel);
            }

            var client = new ClientResult<Client>();
            var existingClient = await _clientService.GetClientAsync(model.ClientName);

            if (!existingClient.Succeeded)
            {
                client = await _clientService.CreateAsync(model.ClientName);
            }
            else if (!existingClient.Succeeded && !client.Succeeded)
            {
                return BadRequest("Client was not created.");
            }
            else
            {
                client = existingClient;
            }

            if (model.Picture != null)
            {
                var relativePath = await _fileService.CreateFile(model.Picture);

                if (relativePath == model.ImageUrl)
                {
                    _fileService.DeleteFile(relativePath);
                    var projectDto = ProjectFactoryMVC.CreateDtoFromEditForm(model);

                    var updatedProject = await _projectService.UpdateAsync(projectDto);
                    if (!updatedProject.Succeeded)
                        return BadRequest($"An error occurred updating the member.\n{updatedProject.StatusCode}\n{updatedProject.ErrorMessage}");

                    return RedirectToAction("Index");
                }

                var picture = await _pictureService.CreateAsync(relativePath);
                if (!picture.Succeeded || picture.Data == null)
                    return BadRequest($"An error occurred creating the new image.\n{picture.StatusCode}\n{picture.ErrorMessage}");

                model.ImageUrl = Url.Content(relativePath);
                var dto = ProjectFactoryMVC.CreateDtoFromEditForm(model);
                dto.PictureId = picture.Data.Id;

                if (existingClient.Data != null)
                {
                    dto.ClientId = existingClient.Data?.Id;
                }
                else if (existingClient == null && client.Data != null)
                {
                    dto.ClientId = client.Data?.Id;
                }

                var result = await _projectService.UpdateAsync(dto);

                if (!result.Succeeded)
                {
                    return BadRequest("Failed to update project");
                }
                var existingMembers = await _pmService.GetProjectMembersAsync(dto.Id);
                if (existingMembers.Any())
                {
                    var remove = await _pmService.DeleteAsync(existingMembers);
                }

                if (!string.IsNullOrEmpty(SelectedIds))
                {
                    //var memberIds = JsonSerializer.Deserialize<List<Guid>>(SelectedIds);
                    var memberIds = SelectedIds
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(Guid.Parse)
                        .ToList();
                    if (memberIds != null)
                    {
                        foreach (var member in memberIds)
                        {
                            await _pmService.AddAsync(new ProjectMemberDto { ProjectId = model.Id, MemberId = member });
                        }
                    }
                }
            }
            else
            {
                model.ImageUrl = model.CurrentUrl;
                var dto = ProjectFactoryMVC.CreateDtoFromEditForm(model);

                var pictureId = await _pictureService.GetPictureIdAsync(model.ImageUrl!);
                dto.PictureId = pictureId.Data;
                dto.Id = model.Id;

                if (existingClient.Data != null)
                {
                    dto.ClientId = existingClient.Data?.Id;
                }
                else if (existingClient == null && client.Data != null)
                {
                    dto.ClientId = client.Data?.Id;
                }

                var result = await _projectService.UpdateAsync(dto);

                if (!result.Succeeded)
                {
                    return BadRequest("Failed to update project");
                }
                var existingMembers = await _pmService.GetProjectMembersAsync(dto.Id);
                if (existingMembers.Any())
                {
                    var remove = await _pmService.DeleteAsync(existingMembers);
                }

                if (!string.IsNullOrEmpty(SelectedIds))
                {
                    //var memberIds = JsonSerializer.Deserialize<List<Guid>>(SelectedIds);
                    var memberIds = SelectedIds
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(Guid.Parse)
                        .ToList();
                    if (memberIds != null)
                    {
                        foreach (var member in memberIds)
                        {
                            await _pmService.AddAsync(new ProjectMemberDto { ProjectId = model.Id, MemberId = member });
                        }
                    }
                }
            }

            var members = await _memberService.GetMemberUsersAsync();
            var clients = await _clientService.GetClientsAsync();
            var cards = await _projectService.GetProjectCardsAsync();

            var viewModel = new ProjectsViewModel
            {
                Cards = cards.Data?.Select(ProjectFactoryMVC.CreateCardFromDomainModel).ToList() ?? [],
                AllMembers = members.Data?.Select(member => new MySelectListItem
                {
                    Value = member.Id.ToString(),
                    Text = $"{member.FirstName} {member.LastName}",
                    ImageUrl = member.ImageUrl
                }).ToList() ?? [],
                Clients = clients.Data?.Select(client => new SelectListItem
                {
                    Value = client.Id.ToString(),
                    Text = client.ClientName
                }).ToList() ?? []
            };

            ModelState.AddModelError("viewModel", "Failed to update project.");
            return View("Index", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var project = await _projectService.GetProjectAsync(id);

            if (!project.Succeeded || project.Data == null || project.Data.ImageUrl == null)
                return NotFound($"Status code: {project.StatusCode}\nError message: \n{project.ErrorMessage}");

            var filePath = Path.Combine(_environment.WebRootPath, project.Data.ImageUrl.TrimStart('/'));
            var result = await _projectService.DeleteAsync(id);

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
                var clients = await _clientService.GetClientsAsync();
                var cards = await _projectService.GetProjectCardsAsync();

                var viewModel = new ProjectsViewModel
                {
                    Cards = cards.Data?.Select(ProjectFactoryMVC.CreateCardFromDomainModel).ToList() ?? [],
                    AllMembers = members.Data?.Select(member => new MySelectListItem
                    {
                        Value = member.Id.ToString(),
                        Text = $"{member.FirstName} {member.LastName}",
                        ImageUrl = member.ImageUrl
                    }).ToList() ?? [],
                    Clients = clients.Data?.Select(client => new SelectListItem
                    {
                        Value = client.Id.ToString(),
                        Text = client.ClientName
                    }).ToList() ?? []
                };

                ModelState.AddModelError("viewModel", "Failed to create member.");
                return View("Index", viewModel);
            }
        }
    }
}
