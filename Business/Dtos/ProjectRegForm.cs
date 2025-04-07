using Business.Models;

namespace Business.Dtos;

public class ProjectRegForm
{
    public string ProjectName { get; set; } = null!;
    public string? ProjectDescription { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid StatusID { get; set; }
    public Guid ClientId { get; set; }
    public decimal Budget { get; set; }
    public string ProjectImage { get; set; } = null!;
    public IEnumerable<ProjectNotesModel> ProjectNotes { get; set; } = [];
}
