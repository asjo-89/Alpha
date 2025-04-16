namespace Alpha_Mvc.Interfaces
{
    public interface IFileService
    {
        Task<string> CreateFile(IFormFile file);
        bool DeleteFile(string filePath);
    }
}