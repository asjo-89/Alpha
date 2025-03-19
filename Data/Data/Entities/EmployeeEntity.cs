using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class EmployeeEntity
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public int AddressId { get; set; }
    public int RoleId { get; set; }
    public int PictureId { get; set; }
}
