using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class CreateRoleFormData
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Role Name", Prompt = "Enter role name")]
    [DataType(DataType.Text)]
    public string RoleName { get; set; } = null!;
}
