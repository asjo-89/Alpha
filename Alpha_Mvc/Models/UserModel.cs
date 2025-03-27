namespace Alpha_Mvc.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string JobTitle { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!; 
    public string ProfilePicture { get; set; } = null!;
}
