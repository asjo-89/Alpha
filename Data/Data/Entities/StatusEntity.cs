using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class StatusEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [DataType(DataType.Text)]
    public string StatusName { get; set; } = null!;


    // Navigation

    public ICollection<ProjectEntity>? Projects { get; set; } = [];
}
