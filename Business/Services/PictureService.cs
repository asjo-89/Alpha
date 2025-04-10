using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Extensions;
using System.Diagnostics;

namespace Business.Services;

public class PictureService(IPictureRepository pictureRepository) : IPictureService
{
    private readonly IPictureRepository _pictureRepository = pictureRepository;

    public async Task<PictureResult> CreateAsync(string url)
    {
        if (string.IsNullOrEmpty(url))
            return new PictureResult { Succeeded = false, StatusCode = 400, ErrorMessage = "No picture url was provided." };

        try
        {
            var started = await _pictureRepository.BeginTransactionAsync();

            PictureEntity entity = new PictureEntity
            {
                ImageUrl = url
            };

            var result = await _pictureRepository.CreateAsync(entity);
            if (!result.Success)
                return new PictureResult { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to save picture." };

            await _pictureRepository.CommitTransactionAsync();
            return new PictureResult { Succeeded = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            var rollback = await _pictureRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new PictureResult { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to save picture: {ex.Message} " };
        }
    }
}
