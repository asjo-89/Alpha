using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class ClientModel
{
    public Guid Id { get; set; }


    [Required(ErrorMessage = "Client name is required.")]
    [Display(Name = "Client Name", Prompt = "Enter client name...")]
    public string ClientName { get; set; } = null!;


    [Required(ErrorMessage = "Phone number is required.")]
    [Display(Name = "Phone Number", Prompt = "Enter phone number...")]
    public string PhoneNumber { get; set; } = null!;


    [Display(Name = "Email", Prompt = "Enter email address...")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }
}
