namespace Data.Contexts.Entities;

public class PictureEntity
{
    public Guid Id { get; set; }
    public string PictureUrl { get; set; } = null!;


    // Relations
    public ICollection<ProjectEntity> Projects { get; set; } = [];
    public ICollection<EmployeeEntity> Employees { get; set; } = [];
}
