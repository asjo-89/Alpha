﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Index(nameof(Email), IsUnique = true)]
public class MemberUserEntity : IdentityUser<Guid>
{
    [ProtectedPersonalData]
    public string? FirstName { get; set; }

    [ProtectedPersonalData]
    public string? LastName { get; set; }
    
    [ProtectedPersonalData]
    public string? JobTitle { get; set; }


    [ProtectedPersonalData]
    [Column(TypeName = "date")]
    public DateOnly? DateOfBirth { get; set; }


    [ProtectedPersonalData]
    public Guid? PictureId { get; set; }





    // Navigation

    public virtual AddressEntity? Address { get; set; } 


    [ForeignKey(nameof(PictureId))]
    public virtual PictureEntity? Picture { get; set; }

    public virtual ICollection<ProjectMemberEntity>? ProjectMembers { get; set; } = [];

    public virtual ICollection<ProjectNoteEntity>? ProjectNotes { get; set; } = [];
}
