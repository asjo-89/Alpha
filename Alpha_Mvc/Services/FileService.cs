using Alpha_Mvc.Interfaces;

namespace Alpha_Mvc.Services;

public class FileService(IWebHostEnvironment environment) : IFileService
{
    private readonly IWebHostEnvironment _environment = environment;

    public async Task<string> CreateFile(IFormFile file)
    {
        var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");
        Directory.CreateDirectory(directoryPath);

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(directoryPath, fileName);
        var relativePath = $"uploads/{fileName}";

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return relativePath ?? "";
    }

    public bool DeleteFile(string filePath)
    {
        if (filePath == null || filePath == "Images/Profiles/Profile2.png") return false;

        File.Delete(filePath);
        return true;
    }
}
