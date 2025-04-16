using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class ProjectNote
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime Created { get; set; }
    public MemberUser Member { get; set; } = null!;
    public Project Project { get; set; } = null!;
}
