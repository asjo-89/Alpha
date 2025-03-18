namespace Business.Dtos;

public class SignUpRegForm
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string StreetAddress { get; set; } = null!;
    public int PostalCode { get; set; }
    public string City { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public int PictureId { get; set; }
}
