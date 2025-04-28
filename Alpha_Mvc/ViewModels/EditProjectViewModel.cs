using Alpha_Mvc.Models;

namespace Alpha_Mvc.ViewModels;

public class EditProjectViewModel
{
    public ProjectCardModel ProjectCard { get; set; } = new();
    public EditProjectFormModel FormModel { get; set; } = new();
}
