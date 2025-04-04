namespace Alpha_Mvc.Models.ViewModels;

public class ProjectsViewModel
{
    public ProjectFormModel Project { get; set; } = new();
    public ProjectModel ProjectModel { get; set; } = new();
    //public IEnumerable<ProjectModel> Projects { get; set; } = [];
}
