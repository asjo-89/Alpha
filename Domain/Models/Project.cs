using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Project
{
    public Guid Id { get; set; }
    public string ProjectTitle { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? Budget { get; set; }
    public DateTime Created { get; set; }
    public string ImageUrl { get; set; } = null!;
    public Guid? ClientId { get; set; }
    public Client Client { get; set; } = new();
    public string StatusName { get; set; } = null!;
    public IEnumerable<MemberUser>? ProjectMembers { get; set; } = [];
    public IEnumerable<ProjectNote>? ProjectNotes { get; set; } = [];
}
