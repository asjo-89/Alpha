namespace Domain.Models;

public class ProjectMember
{
    public Guid ProjectId { get; set; }
    public Guid MemberId { get; set; }
}
