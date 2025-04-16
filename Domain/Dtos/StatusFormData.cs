using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class StatusFormData
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Status  name", Prompt = "Enter status name")]
    [DataType(DataType.Text)]
    public string StatusName { get; set; } = null!;
}
