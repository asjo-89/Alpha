using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class MemberEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, ProtectedPersonalData]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;

    [Required, ProtectedPersonalData]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;

    [Required, ProtectedPersonalData]
    [DataType(DataType.Text)]
    public string JobTitle { get; set; } = null!;

    [Required, ProtectedPersonalData]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;


    [Required, ProtectedPersonalData]
    public Guid AddressId { get; set; }
    public Guid? PictureId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? ProjectNoteId { get; set; }





    // Navigation

    public AddressEntity Address { get; set; } = null!;
    public PictureEntity? Picture { get; set; }
    public ICollection<ProjectMemberEntity>? Projects { get; set; } = [];
    public ICollection<PictureEntity>? ProjectNotes { get; set; } = [];


}
