using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using System.Diagnostics;

namespace Business.Services;

public class ProjectNoteService(IProjectNoteRepository projectNoteRepository) : IProjectNoteService
{
    private readonly IProjectNoteRepository _projectNoteRepository = projectNoteRepository;

    public async Task<ProjectNoteResult> CreateAsync(ProjectNoteFormData formData)
    {
        if (formData == null)
            return new ProjectNoteResult { Succeeded = false, StatusCode = 400, ErrorMessage = "All required fields must be completed." };

        try
        {
            var started = await _projectNoteRepository.BeginTransactionAsync();

            var entity = formData.MapTo<ProjectNoteEntity>();

            var result = await _projectNoteRepository.CreateAsync(entity);
            if (!result.Success)
                return new ProjectNoteResult { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to create note." };

            await _projectNoteRepository.CommitTransactionAsync();
            return new ProjectNoteResult { Succeeded = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            var rollback = await _projectNoteRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new ProjectNoteResult { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to create note: {ex.Message} " };
        }
    }

    public async Task<ProjectNoteResult> UpdateAsync(ProjectNoteFormData formData)
    {
        if (formData == null)
            return new ProjectNoteResult { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields are not completed." };

        var entity = formData.MapTo<ProjectNoteEntity>();

        try
        {
            await _projectNoteRepository.BeginTransactionAsync();

            var result = await _projectNoteRepository.UpdateAsync(entity);

            if (!result.Success)
                return new ProjectNoteResult { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to update note." };

            await _projectNoteRepository.CommitTransactionAsync();
            return new ProjectNoteResult { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            var rollback = await _projectNoteRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new ProjectNoteResult { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to update note: {ex.Message} " };
        }
    }


    public async Task<ProjectNoteResult> DeleteAsync(ProjectNoteFormData formData)
    {
        var entity = formData.MapTo<ProjectNoteEntity>();

        try
        {
            await _projectNoteRepository.BeginTransactionAsync();

            var result = await _projectNoteRepository.DeleteAsync(entity);
            if (!result.Success)
                return new ProjectNoteResult { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to delete note." };

            await _projectNoteRepository.CommitTransactionAsync();

            return new ProjectNoteResult { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            var rollback = await _projectNoteRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new ProjectNoteResult { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to delete note: {ex.Message} " };
        }
    }
}
