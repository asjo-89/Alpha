using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<ProjectResult<bool>> CreateAsync(ProjectFormData formData)
    {
        if (formData == null)
            return new ProjectResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields must be completed.", Data = false };

        var entity = formData.MapTo<ProjectEntity>();

        try
        {
            await _projectRepository.BeginTransactionAsync();

            var result = await _projectRepository.CreateAsync(entity);

            if (!result.Success)
                return new ProjectResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to create project.", Data = false };

            await _projectRepository.CommitTransactionAsync();

            return new ProjectResult<bool> { Succeeded = true, StatusCode = 201, Data = true };
        }
        catch (Exception ex)
        {
            var rollback = await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new ProjectResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to create project: {ex.Message} ", Data = false };
        }
    }


    public async Task<ProjectResult<IEnumerable<Project>>> GetMemberUsersAsync()
    {
        var result = await _projectRepository.GetAllAsync(
            orderByDescending: false,
            orderBy: x => new { x.Created, x.Status.StatusName, x.ProjectTitle },
            filterBy: null!);


        return result.Success
            ? new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Data = result.Data?.Select(entity => entity.MapTo<Project>()) }
            : new ProjectResult<IEnumerable<Project>> { Succeeded = false, StatusCode = 404, ErrorMessage = "No projects was found." };
    }


    public async Task<ProjectResult<Project>> GetMemberUserAsync(string value)
    {
        var result = await _projectRepository.GetAsync(
            filterBy: x => x.ProjectTitle.ToLower() == value.ToLower() || x.Description == value || x.Status.StatusName.ToLower() == value.ToLower(),
            includes: null!);

        return result.Success
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Data = result.Data?.MapTo<Project>() }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, ErrorMessage = "No project was found." };
    }


    public async Task<ProjectResult<bool>> UpdateAsync(ProjectFormData formData)
    {
        if (formData == null)
            return new ProjectResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields are not completed.", Data = false };

        var entity = formData.MapTo<ProjectEntity>();

        try
        {
            await _projectRepository.BeginTransactionAsync();

            var result = await _projectRepository.UpdateAsync(entity);

            if (!result.Success)
                return new ProjectResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to update project.", Data = false };

            await _projectRepository.CommitTransactionAsync();

            return new ProjectResult<bool> { Succeeded = true, StatusCode = 200, Data = true };
        }
        catch (Exception ex)
        {
            var rollback = await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new ProjectResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to update project: {ex.Message}", Data = false };
        }
    }


    public async Task<ProjectResult<bool>> DeleteAsync(ProjectFormData formData)
    {
        var entity = formData.MapTo<ProjectEntity>();

        try
        {
            await _projectRepository.BeginTransactionAsync();

            var result = await _projectRepository.DeleteAsync(entity);
            if (!result.Success)
                return new ProjectResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to delete project.", Data = false };

            await _projectRepository.CommitTransactionAsync();

            return new ProjectResult<bool> { Succeeded = true, StatusCode = 200, Data = true };
        }
        catch (Exception ex)
        {
            var rollback = await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new ProjectResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to delete project: {ex.Message}", Data = false };
        }
    }
}
