using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public Guid? ClientId { get; set; }
    public Guid? ProjectMemberId { get; set; }
    public Guid? ProjectNoteId { get; set; }





    // Navigation

    [ForeignKey(nameof(PictureId))]
    public PictureEntity Picture { get; set; } = null!;


    [ForeignKey(nameof(ClientId))]
    public ClientEntity? Client { get; set; } = null!;


    [ForeignKey(nameof(ProjectMemberId))]
    public ICollection<ProjectMemberEntity>? ProjectMembers { get; set; } = [];


    [ForeignKey(nameof(ProjectNoteId))]
    public ICollection<ProjectNoteEntity>? ProjectNotes { get; set; } = [];
}
