using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class StatusEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(10)")]
    public string StatusName { get; set; } = null!;


    //Relations
    public ICollection<ProjectEntity> Projects { get; set; } = [];
}
