using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectMemberEntity
{
    public Guid MemberId { get; set; }
    public Guid ProjectId { get; set; }



    // Navigation


    [ForeignKey(nameof(MemberId))]
    public MemberEntity Member { get; set; } = null!;

    [ForeignKey(nameof(ProjectId))]
    public ProjectEntity Project { get; set; } = null!;
}
