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
    public string Picture { get; set; } = null!;
    public Client Client { get; set; } = null!;
    public Status Status { get; set; } = null!;
    public IEnumerable<MemberUser>? ProjectMembers { get; set; } = [];
    public IEnumerable<ProjectNote>? ProjectNotes { get; set; } = [];
}
