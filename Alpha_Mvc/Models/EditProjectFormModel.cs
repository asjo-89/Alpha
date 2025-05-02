using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class EditProjectFormModel
{
    public Guid Id { get; set; }

    public IFormFile? Picture { get; set; }
    public string? ImageUrl { get; set; }
    public string? CurrentUrl { get; set; }

    [Required(ErrorMessage = "You need to enter a name for the project.")]
    [Display(Name = "Project Title", Prompt = "Enter a title...")]
    public string ProjectTitle { get; set; } = null!;

    [Display(Name = "Client")]
    public Guid? ClientId { get; set; }
    public string ClientName { get; set; } = null!;


    [Required(ErrorMessage = "You need to enter a description.")]
    [Display(Name = "Description", Prompt = "Enter a description...")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "You need to enter a start date.")]
    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "You need to enter an end date.")]
    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "You need to enter a budget.")]
    [Display(Name = "Budget", Prompt = "Enter a budget...")]
    public decimal? Budget { get; set; }



    //public List<string> SelectedIds { get; set; } = [];
    public List<MemberUser> MemberUsers { get; set; } = [];

    //[Required(ErrorMessage = "You need to select at least one member.")]
    //[Display(Name = "Members", Prompt = "Choose members...")]
    //public List<Guid> SelectedMembers { get; set; } = [];

}