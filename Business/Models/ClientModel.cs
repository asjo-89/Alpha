namespace Business.Models;

public class ClientModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string Phone { get; set; } = null!;
}
