using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

[Index(nameof(StreetAddress), IsUnique = true)]
public class AddressEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, ProtectedPersonalData]
    public string StreetAddress { get; set; } = null!;


    [Required, ProtectedPersonalData]
    public int PostalCode { get; set; }


    [Required, ProtectedPersonalData]
    public string City { get; set; } = null!;





    // Navigation

    public virtual ICollection<MemberUserEntity> Members { get; set; } = [];
}
