namespace Data.Entities;

public class ProjectMemberEntity
{
    public Guid MemberId { get; set; }
    public Guid ProjectId { get; set; }



    // Navigation

    public MemberEntity Member { get; set; } = null!;
    public ProjectEntity Project { get; set; } = null!;
}
