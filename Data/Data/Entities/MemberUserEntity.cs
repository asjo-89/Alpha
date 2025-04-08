using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Index(nameof(Email), IsUnique = true)]
public class MemberUserEntity : IdentityUser<Guid>
{
    [Required, ProtectedPersonalData]
    public string FirstName { get; set; } = null!;

    [Required, ProtectedPersonalData]
    public string LastName { get; set; } = null!;

    [ProtectedPersonalData]
    public string? JobTitle { get; set; }


    [ProtectedPersonalData]
    [Column(TypeName = "date")]
    public DateOnly? DateOfBirth { get; set; }


    [Required, ProtectedPersonalData]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;


    [Required, ProtectedPersonalData]
    public Guid AddressId { get; set; }
    public Guid? PictureId { get; set; }





    // Navigation

    [ForeignKey(nameof(AddressId))]
    public virtual AddressEntity Address { get; set; } = null!;


    [ForeignKey(nameof(PictureId))]
    public virtual PictureEntity? Picture { get; set; }

    public virtual ICollection<ProjectMemberEntity>? ProjectMembers { get; set; } = [];

    public virtual ICollection<ProjectNoteEntity>? ProjectNotes { get; set; } = [];
}
