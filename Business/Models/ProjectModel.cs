namespace Business.Models;

public class ProjectModel
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; } = null!;
    public string ProjectDescription { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Budget { get; set; }
    public string ClientName { get; set; } = null!;
    public string StatusName { get; set; } = null!;
    public string PictureUrl { get; set; } = null!;
    public IEnumerable<MemberModel> Members { get; set; } = [];
    public IEnumerable<ProjectNotesModel>? ProjectNotes { get; set; } = [];

}
