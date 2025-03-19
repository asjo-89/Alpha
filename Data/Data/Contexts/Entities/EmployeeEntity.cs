using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Contexts.Entities;

public class EmployeeEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string Password { get; set; } = null!;
    public Guid AddressId { get; set; }
    public Guid RoleId { get; set; }
    public Guid PictureId { get; set; }


    // Relations
    public ICollection<ProjectEmployeeEntity> Projects { get; set; } = [];

    [ForeignKey(nameof(AddressId))]
    public AddressEntity Address { get; set; } = null!;

    [ForeignKey(nameof(RoleId))]
    public RoleEntity Role { get; set; } = null!;

    [ForeignKey(nameof(PictureId))]
    public PictureEntity Picture {  get; set; } = null!;
}
