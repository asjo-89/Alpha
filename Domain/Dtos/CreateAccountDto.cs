using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos
{
    public class CreateAccountDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public Guid? PictureId { get; set; }
    }
}
