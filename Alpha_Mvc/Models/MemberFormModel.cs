using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class MemberFormModel
{
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


    [Required(ErrorMessage = "Phone number is required.")]
    [Display(Name = "Phone number", Prompt = "Enter phone number...")]
    [RegularExpression(@"^(?:\+46|0)([ ]?)(\d{1,3})([ -]?)(\d{2,3})([ -]?)(\d{2})([ -]?)(\d{2})$", ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; } = null!;


    [Required(ErrorMessage = "Job title is required.")]
    [Display(Name = "Job title", Prompt = "Enter job title...")]
    public string JobTitle { get; set; } = null!;


    [Required(ErrorMessage = "Street address is required.")]
    [Display(Name = "Street address", Prompt = "Enter street address...")]
    public string StreetAddress { get; set; } = null!;


    [Required(ErrorMessage = "Postal code is required.")]
    [Display(Name = "Postal Code", Prompt = "Enter postal code...")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Only digits allowed.")]
    public string PostalCode { get; set; } = null!;


    [Required(ErrorMessage = "City is required.")]
    [Display(Name = "City", Prompt = "Enter city...")]
    public string City { get; set; } = null!;


    [Required(ErrorMessage = "Day is required.")]
    public int BirthDay { get; set; }

    [Required(ErrorMessage = "Month is required.")]
    public int BirthMonth { get; set; }

    [Required(ErrorMessage = "Year is required.")]
    public int BirthYear { get; set; }

    public DateOnly DateOfBirth { get; set; } 

    [Required(ErrorMessage = "to select a profile image.")]
    [DataType(DataType.ImageUrl)]
    public IFormFile ProfileImage { get; set; } = null!;
    public string? ImageUrl { get; set; }


    [Required(ErrorMessage = "Role is required.")]
    public Guid? RoleId { get; set; }
}
