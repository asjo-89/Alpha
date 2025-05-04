using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string ProjectTitle { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime? StartDate { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }

    [Precision(18, 2)]
    public decimal? Budget {  get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    [Required]  
    public string StatusName { get; set; } = null!;



    [Required]
    public Guid PictureId { get; set; }

    [Required]
    public Guid ClientId { get; set; }




    // Navigation

    [ForeignKey(nameof(PictureId))]
    public virtual PictureEntity Picture { get; set; } = null!;


    [ForeignKey(nameof(ClientId))]
    public virtual ClientEntity Client { get; set; } = null!;


    public virtual ICollection<ProjectMemberEntity> ProjectMembers { get; set; } = [];
    public virtual ICollection<ProjectNoteEntity>? ProjectNotes { get; set; } = [];
}
