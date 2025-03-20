using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class RoleEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(30)")]
    public string RoleName { get; set; } = null!;


    // Relations
    public ICollection<EmployeeEntity> Employees { get; set; } = [];
}
