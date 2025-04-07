using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class PictureEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [DataType(DataType.ImageUrl)]
    public string ImageUrl { get; set; } = null!;

    public Guid? UserId { get; set; }





    // Navigation

    public ICollection<MemberEntity>? MemberEntities { get; set; } = [];
    public ICollection<ProjectEntity>? ProjectEntities { get; set; } = [];
}
