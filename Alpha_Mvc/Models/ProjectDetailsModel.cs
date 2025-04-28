using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class ProjectDetailsModel
{
    public Guid Id { get; set; }

    [Display(Name = "Project Title")]
    public string ProjectTitle { get; set; } = null!;

    [Display(Name = "Description")]
    public string Description { get; set; } = null!;


    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }


    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }


    [Display(Name = "Client")]
    public ClientModel Client { get; set; } = new();

    public string StatusName { get; set; } = null!;

    [Display(Name = "Budget")]
    public decimal Budget { get; set; }

    public string ImageUrl { get; set; } = null!;

    [Display(Name = "Members")]
    public List<MemberUserModel> Members { get; set; } = [];

    [Display(Name = "Notes")]
    public List<ProjectNotesModel> ProjectNotes { get; set; } = [];
}
