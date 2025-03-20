using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string ProjectName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string ProjectDescription { get; set; } = null!;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Budget { get; set; }

    [Required]
    public Guid ClientId { get; set; }

    [Required]
    public Guid StatusId { get; set; }

    [Required]
    public Guid PictureId { get; set; }


    // Relations
    public ICollection<ProjectEmployeeEntity> ProjectEmployees { get; set; } = [];
    public ICollection<ProjectNoteEntity> Notes { get; set; } = [];

    [ForeignKey(nameof(ClientId))]
    public ClientEntity Client { get; set; } = null!;

    [ForeignKey(nameof(StatusId))]
    public StatusEntity Status { get; set; } = null!;

    [ForeignKey(nameof(PictureId))]
    public PictureEntity Picture { get; set; } = null!;
}
