using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class CreateStatusFormData
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Status  name", Prompt = "Enter status name")]
    [DataType(DataType.Text)]
    public string StatusName { get; set; } = null!;
}
