using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class ProjectNotesModel
{
    public Guid Id { get; set; }


    [Required(ErrorMessage = "Note description is required.")]
    [Display(Name = "Note description", Prompt = "Enter your notes here...")]
    public string Note { get; set; } = null!;

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public Guid EmployeeId { get; set; }

    [Required]
    public Guid ProjectId { get; set; }
}
