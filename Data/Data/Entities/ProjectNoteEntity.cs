using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectNoteEntity
{
    public Guid Id { get; set; }
    public string Note { get; set; } = null!;
    public Guid EmployeeId { get; set; }
    public Guid ProjectId { get; set; }


    // Relations
    [ForeignKey(nameof(EmployeeId))]
    public EmployeeEntity Employee { get; set; } = null!;

    [ForeignKey(nameof(ProjectId))]
    public ProjectEntity Project {  get; set; } = null!;
}
