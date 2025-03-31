using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class CreateMemberFormModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [Display(Name = "First Name", Prompt = "Enter first name...")]
    public string FirstName { get; set; } = null!;


    [Required(ErrorMessage = "Last name is required.")]
    [Display(Name = "Last Name", Prompt = "Enter last name...")]
    public string LastName { get; set; } = null!;


    [Required(ErrorMessage = "Email is required.")]
    [Display(Name = "Email", Prompt = "Enter email address...")]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "Phone number is required.")]
    [Display(Name = "Phone number", Prompt = "Enter phone number...")]
    public string PhoneNumber { get; set; } = null!;


    [Required(ErrorMessage = "Job title is required.")]
    [Display(Name = "Job title", Prompt = "Enter job title...")]
    public string JobTitle { get; set; } = null!;


    [Required(ErrorMessage = "Street address is required.")]
    [Display(Name = "Street address", Prompt = "Enter street address...")]
    public string StreetAddress { get; set; } = null!;


    [Required(ErrorMessage = "Postal code is required.")]
    [Display(Name = "Postal Code", Prompt = "Enter postal code...")]
    public int PostalCode { get; set; }


    [Required(ErrorMessage = "City is required.")]
    [Display(Name = "City", Prompt = "Enter city...")]
    public string City { get; set; } = null!;


    public int BirthDay { get; set; }
    public int BirthMonth { get; set; }
    public int BirthYear { get; set; }

    [Required(ErrorMessage = "Date of birth is required.")]
    [Display(Name = "Date Of Birth", Prompt = "Enter date of birth...")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "You need to select a profile image.")]
    [DataType(DataType.ImageUrl)]
    public IFormFile ProfileImage { get; set; } = null!;
}
