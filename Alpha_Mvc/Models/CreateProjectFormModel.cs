using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class CreateProjectFormModel
{
    [Required(ErrorMessage = "You need to select a picture.")]
    public IFormFile Picture { get; set; } = null!;
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "You need to enter a name for the project.")]
    [Display(Name = "Project Title", Prompt = "Enter a title...")]
    public string ProjectTitle { get; set; } = null!;

    [Required(ErrorMessage = "You need to select a client.")]
    [Display(Name = "Client", Prompt = "Choose a client...")]
    public string ClientName { get; set; } = null!;
    //public Guid ClientId { get; set; }

    [Required(ErrorMessage = "You need to enter a description.")]
    [Display(Name = "Description", Prompt = "Enter a description...")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "You need to enter a start date.")]
    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; } = null!;

    [Required(ErrorMessage = "You need to enter an end date.")]
    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; } = null!;

    [Required(ErrorMessage = "You need to enter a budget.")]
    [Display(Name = "Budget", Prompt = "Enter a budget...")]
    [RegularExpression(@"^\d+$", ErrorMessage ="Only digits allowed.")]
    public decimal? Budget { get; set; }


    [Required(ErrorMessage = "You have to select member(s).")]
    public List<string> SelectedIds { get; set; } = [];

    public string? ErrorMessage { get; set; }

    //[Required(ErrorMessage = "You need to select at least one member.")]
    //[Display(Name = "Members", Prompt = "Choose members...")]
    //public List<Guid> SelectedMembers { get; set; } = [];

    //public List<MySelectListItem>? AllMembers { get; set; } = [];

    //public List<SelectListItem>? Clients { get; set; } = [];
}
