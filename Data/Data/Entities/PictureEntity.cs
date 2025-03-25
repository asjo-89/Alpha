using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class PictureEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string PictureUrl { get; set; } = null!;


    // Relations
    public ICollection<ProjectEntity> Projects { get; set; } = [];
    public ICollection<EmployeeEntity> Employees { get; set; } = [];
}
