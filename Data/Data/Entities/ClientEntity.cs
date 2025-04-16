using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

[Index(nameof(ClientName), IsUnique = true)]
public class ClientEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string ClientName { get; set; } = null!;

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }




    // Navigation

    public virtual ICollection<ProjectEntity>? Projects { get; set; } = [];
}
