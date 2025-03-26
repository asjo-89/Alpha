using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectNoteEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Note { get; set; } = null!;

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public string EmployeeId { get; set; } = null!;

    [Required]
    public Guid ProjectId { get; set; }


    // Relations
    [ForeignKey(nameof(EmployeeId))]
    public MemberUserEntity Employee { get; set; } = null!;

    [ForeignKey(nameof(ProjectId))]
    public ProjectEntity Project {  get; set; } = null!;
}
