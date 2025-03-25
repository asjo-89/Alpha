using Microsoft.AspNetCore.Http;

namespace Business.Dtos;

public class PictureRegForm
{
    public IFormFile ImageUrl { get; set; } = null!;
}
