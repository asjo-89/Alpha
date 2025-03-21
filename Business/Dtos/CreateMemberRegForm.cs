namespace Business.Dtos;

public class CreateMemberRegForm
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string StandardPassword { get; set; } = null!;
    //Profilbild
}
