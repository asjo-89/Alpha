using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class CreateAccountRegForm
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
    public string Password { get; set; } = null!;
}
