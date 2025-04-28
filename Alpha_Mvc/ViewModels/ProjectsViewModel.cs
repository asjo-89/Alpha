using Alpha_Mvc.Models;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alpha_Mvc.ViewModels;

public class ProjectsViewModel
{
    public ProjectDetailsModel Project { get; set; } = new();
    public List<ProjectCardModel> Cards { get; set; } = new();
    public CreateProjectFormModel CreateProjectForm { get; set; } = new();
    public EditProjectFormModel EditProjectForm { get; set; } = new();


    public List<MySelectListItem>? AllMembers { get; set; } = [];
    public List<MemberUser> MemberUsers { get; set; } = [];
    public List<SelectListItem>? Clients { get; set; } = [];
}
