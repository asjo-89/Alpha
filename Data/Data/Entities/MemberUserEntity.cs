using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class MemberUserEntity : IdentityUser
{
    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    [ProtectedPersonalData]
    [Column(TypeName = "varchar(20)")]
    public new string? PhoneNumber { get; set; }

    [ProtectedPersonalData]
    [Required]
    [Column(TypeName = "varchar(250)")]
    public string EmailAddress { get; set; } = null!;

    [ProtectedPersonalData]
    public DateOnly? DateOfBirth { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string Password { get; set; } = null!;

    [ProtectedPersonalData]
    [Required]
    public Guid AddressId { get; set; }

    [Required]
    public Guid PictureId { get; set; }


    // Relations
    public ICollection<ProjectEmployeeEntity> ProjectEmployees { get; set; } = [];
    public ICollection<ProjectNoteEntity> Notes { get; set; } = [];

    [ForeignKey(nameof(AddressId))]
    public AddressEntity Address { get; set; } = null!;

    [ForeignKey(nameof(PictureId))]
    public PictureEntity Picture { get; set; } = null!;

}
