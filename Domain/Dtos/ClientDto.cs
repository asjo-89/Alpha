using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class ClientDto
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Client Name", Prompt = "Enter client name")]
    [DataType(DataType.Text)]
    public string ClientName { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}")]
    public string Email { get; set; } = null!;


    [Display(Name = "Phone number", Prompt = "Enter phone number")]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^(?:(?:\+|00)(?:[1-9]\d{0,2})[ ]?)?(?:\(?\d{1,4}\)?)?(?>[ \-])?(?:\d{1,4})(?>[ ])?(?:\d{1,4})?(?>[ ])?(?:\d{1,9})$")]
    public string? PhoneNumber { get; set; }
}
