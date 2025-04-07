using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [DataType(DataType.Text)]
    public string ProjectTitle { get; set; } = null!;

    [DataType(DataType.Text)]
    public string? Description { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }

    public decimal? Budget {  get; set; }




    [Required]
    public Guid PictureId { get; set; }

    public Guid ClientId { get; set; }
    public Guid? MemberId { get; set; }
    public Guid? ProjectNoteId { get; set; }





    // Navigation

    public PictureEntity Picture { get; set; } = null!;
    public ClientEntity Client { get; set; } = null!;
    public ICollection<ProjectMemberEntity>? Member { get; set; } = [];
    public ICollection<ProjectNoteEntity>? ProjectNotes { get; set; } = [];
}
