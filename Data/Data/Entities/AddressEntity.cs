using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class AddressEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, ProtectedPersonalData]
    [DataType(DataType.Text)]
    public string StreetAddress { get; set; } = null!;


    [Required, ProtectedPersonalData]
    [DataType(DataType.PostalCode)]
    public int PostalCode { get; set; }


    [Required, ProtectedPersonalData]
    [DataType(DataType.Text)]
    public string City { get; set; } = null!;





    // Navigation

    public ICollection<MemberEntity> Members { get; set; } = [];
}
