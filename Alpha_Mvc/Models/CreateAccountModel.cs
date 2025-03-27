using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class CreateAccountModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [Display(Name = "First Name", Prompt = "Enter first name...")]
    public string FirstName { get; set; } = null!;


    [Required(ErrorMessage = "Last name is required.")]
    [Display(Name = "Last Name", Prompt = "Enter last name...")]
    public string LastName { get; set; } = null!;


    [Required(ErrorMessage = "Email address is required.")]
    [Display(Name = "Email", Prompt = "Enter email address...")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "Password is required.")]
    [Display(Name = "Password", Prompt = "Enter a password...")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain: " +
        "one lower case and one upper case letter, one digit, one special character.")]
    public string Password { get; set; } = null!;


    [Required(ErrorMessage = "Confirm password is required.")]
    [Display(Name = "Confirm Password", Prompt = "Confirm password...")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The passwords does not match.")]
    public string ConfirmPassword { get; set; } = null!;


    [Range(typeof(bool), "true", "true", ErrorMessage = "You need to accept the terms and conditions.")]
    public bool TermsAndConditions { get; set; } = false;
}
