using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class MemberUserEntity : IdentityUser
{
    [Required, ProtectedPersonalData]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;


    [Required, ProtectedPersonalData]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;


    [Required, ProtectedPersonalData]
    [DataType(DataType.Text)]
    public string JobTitle { get; set; } = null!;
}
