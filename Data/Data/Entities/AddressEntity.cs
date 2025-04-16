using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class AddressEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, ProtectedPersonalData]
    public string StreetAddress { get; set; } = null!;


    [Required, ProtectedPersonalData]
    public string PostalCode { get; set; } = null!;


    [Required, ProtectedPersonalData]
    public string City { get; set; } = null!;

    public Guid MemberUserId { get; set; }





    // Navigation

    [ForeignKey(nameof(MemberUserId))]
    public virtual MemberUserEntity? Member { get; set; }
}
