using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Domain.Dtos;

public class ProjectFormData
{
    public string ProjectTitle { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? Budget { get; set; }
    public string PrictureUrl { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string StatusName { get; set; } = null!;
    public IEnumerable<MemberUser> Members { get; set; } = [];
    public IEnumerable<ProjectNote> ProjectNotes { get; set; } = [];
}
