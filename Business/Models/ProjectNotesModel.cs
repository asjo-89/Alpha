namespace Business.Models;

public class ProjectNotesModel
{
    public Guid Id { get; set; }
    public string Note { get; set; } = null!;
    public DateTime Created { get; set; }
    public string Member { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
}
