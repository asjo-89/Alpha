using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }


    [Required, ProtectedPersonalData]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;


    [Required, ProtectedPersonalData]
    public Guid AddressId { get; set; }
    public Guid? PictureId { get; set; }
    public Guid? ProjectMemberId { get; set; }
    public Guid? ProjectNoteId { get; set; }





    // Navigation

    [ForeignKey(nameof(AddressId))]
    public AddressEntity Address { get; set; } = null!;


    [ForeignKey(nameof(PictureId))]
    public PictureEntity? Picture { get; set; }


    [ForeignKey(nameof(ProjectMemberId))]
    public ICollection<ProjectMemberEntity>? ProjectMembers { get; set; } = [];


    [ForeignKey(nameof(ProjectNoteId))]
    public ICollection<PictureEntity>? ProjectNotes { get; set; } = [];


}
