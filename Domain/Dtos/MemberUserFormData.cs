using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class MemberUserFormData
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "First Name", Prompt = "Enter first name")]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [Display(Name = "Last Name", Prompt = "Enter last name")]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}")]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [Display(Name = "Phone Number", Prompt = "Enter phone number")]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^(?:(?:\+|00)(?:[1-9]\d{0,2})[ ]?)?(?:\(?\d{1,4}\)?)?(?>[ \-])?(?:\d{1,4})(?>[ ])?(?:\d{1,4})?(?>[ ])?(?:\d{1,9})$")]
    public string PhoneNumber { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [Display(Name = "Date Of Birth")]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }


    [Required(ErrorMessage = "Required")]
    [Display(Name = "Job Title", Prompt = "Enter job title")]
    [DataType(DataType.Text)]
    public string JobTitle { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [Display(Name = "Role", Prompt = "Select role")]
    [DataType(DataType.Text)]
    public string RoleName { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [Display(Name = "Street Address", Prompt = "Enter street address")]
    [DataType(DataType.Text)]
    public string StreetAddress { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [Display(Name = "Postal Code", Prompt = "Enter postal code")]
    [DataType(DataType.PostalCode)]
    public string PostalCode { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [Display(Name = "City", Prompt = "Enter city")]
    [DataType(DataType.Text)]
    public string City { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [DataType(DataType.ImageUrl)]
    public string Picture { get; set; } = null!;
}
