namespace Business.Dtos;

public class ClientRegForm
{
    public string ClientName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Email { get; set; }
}
