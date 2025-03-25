namespace Business.Dtos;

public class ProjectNoteRegForm
{
    public string Description { get; set; } = null!;
    public DateTime Created { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid ProjectId { get; set; }
}
