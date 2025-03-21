namespace Business.Dtos;

public class CreateProjectRegForm
{
    public string ProjectName { get; set; } = null!;
    public string? ProjectDescription { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Budget { get; set; } = null!;
    public ClientModel Client { get; set; } = null!;
    public StatusModel Status { get; set; } = null!;
    public PictureModel Picture { get; set; } = null!;
}
