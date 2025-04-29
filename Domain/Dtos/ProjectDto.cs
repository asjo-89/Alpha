namespace Domain.Dtos;

public class ProjectDto
{
    public Guid? Id { get; set; }
    public string ProjectTitle { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public string? ImageUrl { get; set; } = null!;
    public Guid? PictureId { get; set; }
    public Guid? ClientId { get; set; }
    public List<MemberUserDto> Members { get; set; } = [];
    public List<ProjectNoteDto> ProjectNotes { get; set; } = [];
}
