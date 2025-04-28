//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel.DataAnnotations;

//namespace Data.Entities;

//[Index(nameof(StatusName), IsUnique = true)]
//public class StatusEntity
//{
//    [Key]
//    public Guid Id { get; set; }

//    [Required]
//    public string StatusName { get; set; } = null!;


//    // Navigation

//    public virtual ICollection<ProjectEntity>? Projects { get; set; } = [];
//}
