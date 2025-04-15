namespace Alpha_Mvc.Dtos
{
    public class CreateAccountFormData
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}
