using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class SignInFormData
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Email Address", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}")]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [Display(Name = "Password", Prompt = "Enter password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$")]
    public string Password { get; set; } = null!;


    [Range(typeof(bool), "true", "true")]
    public bool IsPersistent { get; set; }
}
