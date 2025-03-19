using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; } = null!;
    public string ProjectDescription { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Budget { get; set; }
    public Guid ClientId { get; set; }
    public Guid StatusId { get; set; }
    public Guid PictureId { get; set; }


    // Relations
    public ICollection<ProjectEmployeeEntity> Employees { get; set; } = [];
    public ICollection<ProjectNoteEntity> Notes { get; set; } = [];

    [ForeignKey(nameof(ClientId))]
    public ClientEntity Client { get; set; } = null!;

    [ForeignKey(nameof(StatusId))]
    public StatusEntity Status { get; set; } = null!;

    [ForeignKey(nameof(PictureId))]
    public PictureEntity Picture { get; set; } = null!;
}
