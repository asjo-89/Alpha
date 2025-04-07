namespace Business.Models;

public class ClientModel
{
    public Guid Id { get; set; }
    public string ClientName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
}
