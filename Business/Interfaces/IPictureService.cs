using Business.Models;

namespace Business.Interfaces
{
    public interface IPictureService
    {
        Task<PictureResult> CreateAsync(string url);
    }
}