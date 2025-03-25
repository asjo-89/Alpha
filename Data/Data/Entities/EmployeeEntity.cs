using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class EmployeeEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(250)")]
    public string EmailAddress { get; set; } = null!;

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string Password { get; set; } = null!;

    [Required]
    public Guid AddressId { get; set; }

    [Required]
    public Guid RoleId { get; set; }

    [Required]
    public Guid PictureId { get; set; }


    // Relations
    public ICollection<ProjectEmployeeEntity> ProjectEmployees { get; set; } = [];
    public ICollection<ProjectNoteEntity> Notes { get; set; } = [];

    [ForeignKey(nameof(AddressId))]
    public AddressEntity Address { get; set; } = null!;

    [ForeignKey(nameof(RoleId))]
    public RoleEntity Role { get; set; } = null!;

    [ForeignKey(nameof(PictureId))]
    public PictureEntity Picture {  get; set; } = null!;
}
