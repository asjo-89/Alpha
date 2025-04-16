using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class ProjectNoteFormData
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Description", Prompt = "Enter your notes")]
    [DataType(DataType.Text)]
    public string Content { get; set; } = null!;

    public Guid MemberId { get; set; }
    public Guid ProjectId { get; set; }
}
