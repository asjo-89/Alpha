using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Contexts.Entities;

public class ProjectNoteEntity
{
    public Guid Id { get; set; }
    public string Note { get; set; } = null!;
    public Guid EmployeeId { get; set; }


    // Relations
    [ForeignKey(nameof(EmployeeId))]
    public EmployeeEntity Employee { get; set; } = null!;
    public ProjectEntity Project {  get; set; } = null!;
}
