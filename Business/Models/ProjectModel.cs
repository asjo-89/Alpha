namespace Business.Models;

public class ProjectModel
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
    public IEnumerable<MemberModel> Members { get; set; } = [];
    public IEnumerable<ProjectNotesModel>? ProjectNotes { get; set; } = [];

}
