namespace Business.Models;

public class ProjectNotesModel
{
    public Guid Id { get; set; }
    public string Note { get; set; } = null!;
    public DateTime Created { get; set; }
    public Guid ProjectId { get; set; }
    public Guid EmployeeId { get; set; }
}
