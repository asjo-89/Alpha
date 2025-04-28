using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Domain.Dtos;

public class ProjectNoteDto
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Description", Prompt = "Enter your notes")]
    [DataType(DataType.Text)]
    public string Content { get; set; } = null!;

    public MemberUser Member { get; set; } = new();
    public Guid ProjectId { get; set; }
}
