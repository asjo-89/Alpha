using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ClientEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [DataType(DataType.Text)]
    public string ClientName { get; set; } = null!;

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }




    // Navigation

    public ICollection<ProjectEntity>? Projects { get; set; } = [];
}
