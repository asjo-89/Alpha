using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class AddressEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string StreetName { get; set; } = null!;

    [Required]
    [Column(TypeName = "int")]
    public int PostalCode { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(20)")]
    public string City { get; set; } = null!;


    // Relations
    public ICollection<EmployeeEntity> Employees { get; set; } = [];
}
