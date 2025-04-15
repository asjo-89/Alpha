using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<StatusResult<bool>> CreateAsync(StatusFormData formData)
    {
        if (formData == null)
            return new StatusResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields must be completed.", Data = false };

        var entity = formData.MapTo<StatusEntity>();
        var exists = await _statusRepository.ExistsAsync(x => x.StatusName == entity.StatusName);

        if (exists.Success)
            return new StatusResult<bool> { Succeeded = false, StatusCode = 409, ErrorMessage = $"{formData.StatusName} already exists.", Data = false };

        try
        {
            await _statusRepository.BeginTransactionAsync();

            var result = await _statusRepository.CreateAsync(entity);

            if (!result.Success)
                return new StatusResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to create status.", Data = false };

            await _statusRepository.CommitTransactionAsync();

            return new StatusResult<bool> { Succeeded = true, StatusCode = 201, Data = true };
        }
        catch (Exception ex)
        {
            var rollback = await _statusRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new StatusResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to create status: {ex.Message}", Data = false };
        }
    }


    public async Task<StatusResult<IEnumerable<Status>>> GetClientsAsync()
    {
        var result = await _statusRepository.GetAllAsync(
            orderByDescending: false,
            orderBy: x => x.StatusName,
            filterBy: null!);


        return result.Success
            ? new StatusResult<IEnumerable<Status>> { Succeeded = true, StatusCode = 200, Data = result.Data?.Select(entity => entity.MapTo<Status>()) }
            : new StatusResult<IEnumerable<Status>> { Succeeded = false, StatusCode = 404, ErrorMessage = "No statuses was found." };
    }


    public async Task<StatusResult<Status>> GetClientAsync(string value)
    {
        var result = await _statusRepository.GetAsync(
            filterBy: x => x.StatusName == value,
            includes: null!);

        return result.Success
            ? new StatusResult<Status> { Succeeded = true, StatusCode = 200, Data = result.Data?.MapTo<Status>() }
            : new StatusResult<Status> { Succeeded = false, StatusCode = 404, ErrorMessage = $"No status matching {value} was found." };
    }


    public async Task<StatusResult<bool>> UpdateAsync(StatusFormData formData)
    {
        if (formData == null)
            return new StatusResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields are not completed.", Data = false };

        var entity = formData.MapTo<StatusEntity>();

        try
        {
            await _statusRepository.BeginTransactionAsync();

            var result = await _statusRepository.UpdateAsync(entity);

            if (!result.Success)
                return new StatusResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to update status.", Data = false };
            await _statusRepository.CommitTransactionAsync();

            return new StatusResult<bool> { Succeeded = true, StatusCode = 200, Data = true };
        }
        catch (Exception ex)
        {
            var rollback = await _statusRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new StatusResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to update status: {ex.Message}", Data = false };
        }
    }


    public async Task<StatusResult<bool>> DeleteAsync(StatusFormData formData)
    {
        var entity = formData.MapTo<StatusEntity>();

        try
        {
            await _statusRepository.BeginTransactionAsync();

            var result = await _statusRepository.DeleteAsync(entity);
            if (!result.Success)
                return new StatusResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to delete status.", Data = false };

            await _statusRepository.CommitTransactionAsync();

            return new StatusResult<bool> { Succeeded = true, StatusCode = 200, Data = true };
        }
        catch (Exception ex)
        {
            var rollback = await _statusRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new StatusResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to delete status: {ex.Message}", Data = false };
        }
    }
}
