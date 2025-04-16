using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectNoteEntity
{
    [Key]
    public Guid Id { get; set; }


    [Required]
    public string Content { get; set; } = null!;

    [Required]
    [Column(TypeName = "date")]
    public DateTime Created { get; set; } = DateTime.Now;




    [Required]
    public Guid MemberId { get; set; }

    [Required]
    public Guid ProjectId { get; set; }


    // Navigation


    [ForeignKey(nameof(MemberId))]
    public virtual MemberUserEntity Member { get; set; } = null!;


    [ForeignKey(nameof(ProjectId))]
    public virtual ProjectEntity Project { get; set; } = null!;
}
