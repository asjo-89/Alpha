using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public class PictureService(IPictureRepository pictureRepository) : IPictureService
{
    private readonly IPictureRepository _pictureRepository = pictureRepository;

    public async Task<PictureResult<Picture>> CreateAsync(string url)
    {
        Console.WriteLine("PictureService");

        if (string.IsNullOrEmpty(url))
            return new PictureResult<Picture> { Succeeded = false, StatusCode = 400, ErrorMessage = "No picture url was provided." };

        try
        {
            var started = await _pictureRepository.BeginTransactionAsync();

            PictureEntity entity = new()
            {
                ImageUrl = url
            };

            var result = await _pictureRepository.CreateAsync(entity);

            if (!result.Success)
                return new PictureResult<Picture> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to save picture." };

            await _pictureRepository.CommitTransactionAsync();

            return new PictureResult<Picture> { Succeeded = true, StatusCode = 201, Data = PictureFactory.CreateModelFromEntity(entity) };
        }
        catch (Exception ex)
        {
            var rollback = await _pictureRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new PictureResult<Picture> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to save picture: {ex.Message} " };
        }
    }

    public async Task<PictureResult> ExistsAsync(string url)
    {
        var exists = await _pictureRepository.ExistsAsync(x => x.ImageUrl == url);

        return exists.Success
            ? new PictureResult { Succeeded = true, StatusCode = 200 }
            : new PictureResult { Succeeded = false, StatusCode = 404, ErrorMessage = $"Picture does not exist. " };

    }

    //public async Task<PictureResult<bool>> DeleteAsync(Picture picture)
    //{
    //    //var picture = await _pictureRepository.GetAsync(
    //    //    filterBy: x => x.Id == id
    //    //    );

    //    if (picture == null)
    //        return new PictureResult<bool> { Succeeded = false, StatusCode = 404, ErrorMessage = $"Picture with id {id} was not found." };

    //    var result = await _pictureRepository.DeleteAsync(picture);

    //    return result.Success
    //        ? new PictureResult<bool> { Succeeded = true, StatusCode = 200, Data = true }
    //        : new PictureResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to delete picture." };
    //}
}
