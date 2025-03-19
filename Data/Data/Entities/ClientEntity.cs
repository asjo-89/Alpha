namespace Data.Entities;

public class ClientEntity
{
    public Guid Id { get; set; }
    public string ClientName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Email { get; set; }


    // Relations
    public ICollection<ProjectEntity> Projects { get; set; } = [];
}
