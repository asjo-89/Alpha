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
    //public string StreetAddress { get; set; } = null!;
    //public int PostalCode { get; set; }
    //public string City { get; set; } = null!;
    //public decimal DateOfBirth { get; set; }
    //public string Password { get; set; } = null!;
    //public int RoleId { get; set; }
    //public int PictureId { get; set; }
}
