namespace Domain.Models;

public class Address
{
    public Guid Id { get; set; }
    public string StreetAddress { get; set; } = null!;
    public int PostalCode { get; set; }
    public string City { get; set; } = null!;
}
