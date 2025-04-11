using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class CreateProjectFormModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "You need to select a picture.")]
    public string ImageUrl { get; set; } = null!;

    [Required(ErrorMessage = "You need to enter a name for the project.")]
    public string ProjectName { get; set; } = null!;

    [Required(ErrorMessage = "You need to select a client.")]
    public string ClientName { get; set; } = null!;

    [Required(ErrorMessage = "You need to enter a description.")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "You need to enter a start date.")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "You need to enter an end date.")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "You need to select at least one member.")]
    public string Members { get; set; } = null!;

    [Required(ErrorMessage = "You need to enter a budget.")]
    public decimal Budget { get; set; }
}
