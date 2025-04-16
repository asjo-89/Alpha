namespace Domain.Models;

public class Address
{
    public Guid Id { get; set; }
    public string StreetAddress { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public Guid MemberUserId { get; set; }
}
