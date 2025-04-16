namespace Domain.Models;

public class Client
{
    public Guid Id { get; set; }
    public string ClientName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public IEnumerable<Project> Projects { get; set; } = [];
}
