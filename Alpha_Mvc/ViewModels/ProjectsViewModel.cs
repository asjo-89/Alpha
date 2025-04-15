using Alpha_Mvc.Models;
using Domain.Models;

namespace Alpha_Mvc.ViewModels;

public class ProjectsViewModel
{
    public ProjectFormModel ProjectForm { get; set; } = new();
    public Project ProjectModel { get; set; } = new();
    public ClientModel ClientModel { get; set; } = new();
    public StatusModel StatusModel { get; set; } = new();
    public ProjectNotesModel ProjectNote { get; set; } = new();


    public IEnumerable<TeamMemberModel> Members { get; set; } = [];
    public IEnumerable<Project> Projects { get; set; } = [];
    public IEnumerable<ProjectNotesModel> ProjectNotes { get; set; } = [];
    public IEnumerable<StatusModel> Statuses { get; set; } = [];
    public IEnumerable<ClientModel> Clients { get; set; } = [];
}
