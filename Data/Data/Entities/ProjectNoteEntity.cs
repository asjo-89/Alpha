using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectNoteEntity
{
    [Key]
    public Guid Id { get; set; }


    [Required]
    [DataType(DataType.Text)]
    public string Content { get; set; } = null!;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Created { get; set; } = DateTime.Now;




    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid ProjectId { get; set; }


    // Navigation

    public MemberEntity Member { get; set; } = null!;
    public ProjectEntity Project { get; set; } = null!;
}
