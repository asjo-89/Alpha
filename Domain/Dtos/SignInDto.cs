using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class SignInDto
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Email Address", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [Display(Name = "Password", Prompt = "Enter password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;


    [Range(typeof(bool), "true", "true")]
    public bool IsPersistent { get; set; }
}
