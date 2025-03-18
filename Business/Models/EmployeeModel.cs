namespace Business.Models;

public class EmployeeModel
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
    public int? RoleId { get; set; }
}
