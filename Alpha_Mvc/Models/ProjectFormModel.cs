using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class ProjectFormModel
{
    [Required(ErrorMessage = "Project name is required.")]
    [Display(Name = "Project Title", Prompt = "Enter a project title...")]
    public string ProjectName { get; set; } = null!;


    [Required(ErrorMessage = "Description is required.")]
    [Display(Name = "Description", Prompt = "Enter a description...")]
    public string ProjectDescription { get; set; } = null!;


    [Required(ErrorMessage = "Start date is required.")]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }


    [Required(ErrorMessage = "End date is required.")]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }


    [Required(ErrorMessage = "Client name is required.")]
    [Display(Name = "Client", Prompt = "--- Select a client ---")]
    public string ClientName { get; set; } = null!;


    [Required(ErrorMessage = "Status is required.")]
    [Display(Name = "Status", Prompt = "--- Select a status ---")]
    public string StatusName { get; set; } = null!;


    [Required(ErrorMessage = "Budget is required.")]
    [Display(Name = "Budget", Prompt = "Enter a budget...")]
    public string Budget {  get; set; } = null!;


    [Required(ErrorMessage = "Image is required.")]
    public IFormFile ProjectImage { get; set; } = null!;
}
