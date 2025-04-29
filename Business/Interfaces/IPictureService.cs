using Business.Models;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IPictureService
    {
        Task<PictureResult<Picture>> CreateAsync(string url);
        Task<PictureResult> ExistsAsync(string url);
        //Task<PictureResult<bool>> DeleteAsync(Picture model);
        Task<PictureResult<Guid>> GetPictureIdAsync(string url);
    }
}