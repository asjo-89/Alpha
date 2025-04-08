using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class PictureEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string ImageUrl { get; set; } = null!;





    // Navigation

    public virtual ICollection<MemberUserEntity>? Members { get; set; } = [];

    public virtual ICollection<ProjectEntity>? ProjectEntities { get; set; } = [];
}
