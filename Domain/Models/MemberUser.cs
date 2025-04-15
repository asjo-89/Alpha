namespace Domain.Models;

public class MemberUser
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? JobTitle { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Address? Address { get; set; }
    public Guid? AddressId { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? PictureId { get; set; }
    public Picture? Picture { get; set; }


}
