namespace Data.Contexts.Entities;

public class StatusEntity
{
    public Guid Id { get; set; }
    public string StatusName { get; set; } = null!;


    //Relations
    public ICollection<ProjectEntity> Projects { get; set; } = [];
}
