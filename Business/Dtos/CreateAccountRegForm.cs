using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class CreateAccountRegForm
{
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address.")]
    public string EmailAddress { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-ö])(?=.*[A-Ö])(?=.*\d)(?=.*[\W_]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least" +
        " one lower and upper case letter, one digit and one special sign.")]
    public string Password { get; set; } = null!;

    //[Required(ErrorMessage = "Required")]
    //[Display(Name = "Confirm Password", Prompt = "Confirm your password...")]
    //[DataType(DataType.Password)]
    //public string ConfirmPassword { get; set; } = null!;

    public bool TermsAndConditions { get; set; }
}
