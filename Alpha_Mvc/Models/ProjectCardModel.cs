using Domain.Dtos;
using Domain.Models;

namespace Alpha_Mvc.Models;

public class ProjectCardModel
{
    public Guid Id { get; set; }
    public string ProjectTitle { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; }
    public string StatusName { get; set; } = null!;
    public decimal? Budget {  get; set; }
    public List<MemberUser> MemberUsers { get; set; } = [];
}
