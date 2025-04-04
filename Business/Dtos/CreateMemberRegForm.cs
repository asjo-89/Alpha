using Microsoft.AspNetCore.Http;

namespace Business.Dtos;

public class CreateMemberRegForm
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string StreetAddress { get; set; } = null!;
    public int PostalCode { get; set; }
    public string City { get; set; } = null!;
    public string JobTitle { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string? PassWord { get; set; }
    public string ProfileImage { get; set; } = null!;
}
