using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IPictureRepository pictureRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IPictureRepository _pictureRepository = pictureRepository;


    public async Task<ProjectResult<Project>> CreateAsync(ProjectDto formData)
    {
        if (formData == null)
            return new ProjectResult<Project> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields must be completed." };

        var entity = ProjectFactory.CreateEntityFromDto(formData);

        try
        {
            await _projectRepository.BeginTransactionAsync();

            var result = await _projectRepository.CreateProjectAsync(entity);

            if (!result.Success)
            {
                await _projectRepository.RollbackTransactionAsync();
                return new ProjectResult<Project> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to create project." };
            }

            await _projectRepository.CommitTransactionAsync();

            return new ProjectResult<Project> { Succeeded = true, StatusCode = 201, Data = result.Data != null ? ProjectFactory.CreateModelFromEntity(result.Data) : new Project() };
        }
        catch (Exception ex)
        {
            var rollback = await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new ProjectResult<Project> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to create project: {ex.Message} " };
        }
    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var result = await _projectRepository.GetAllAsync(
            orderByDescending: false,
            orderBy: x => x.Created,
            filterBy: null!);


        return result.Success
            ? new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Data = result.Data?.Select(entity => ProjectFactory.CreateModelFromEntity(entity)) }
            : new ProjectResult<IEnumerable<Project>> { Succeeded = false, StatusCode = 404, ErrorMessage = "No projects was found." };
    }

    public async Task<ProjectResult<List<Project>>> GetProjectsWithDetailsAsync()
    {
        var result = await _projectRepository.GetAllAsync(
            selector: p => new Project
            {
                Id = p.Id,
                ProjectTitle = p.ProjectTitle,
                Description = p.Description ?? "",
                Created = p.Created,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Budget = p.Budget ?? null!,
                Client = new Client
                {
                    Id = p.Client.Id,
                    ClientName = p.Client.ClientName,
                    Email = p.Client.Email,
                    PhoneNumber = p.Client.PhoneNumber,
                },
                StatusName = p.StatusName,
                ProjectNotes = p.ProjectNotes != null ? p.ProjectNotes.Select(pn => new ProjectNote
                {
                    Id = pn.Id,
                    Content = pn.Content,
                    ProjectId = p.Id,
                    Member = new MemberUser
                    {
                        Id = pn.Member.Id,
                        FirstName = pn.Member.FirstName ?? "",
                        LastName = pn.Member.LastName ?? ""
                    },
                    Created = pn.Created
                }).ToList() : null,
                ProjectMembers = p.ProjectMembers != null ? p.ProjectMembers.Select(pm => new MemberUser
                {
                    Id = pm.Member.Id,
                    FirstName = pm.Member.FirstName ?? "",
                    LastName = pm.Member.LastName ?? "",
                    Email = pm.Member.Email ?? "",
                    PhoneNumber = pm.Member.PhoneNumber ?? "",
                    PictureId = pm.Member.PictureId
                }).ToList() : null
            });

        return result.Success && result.Data != null
            ? new ProjectResult<List<Project>> { Succeeded = true, StatusCode = 200, Data = result.Data.ToList() }
            : new ProjectResult<List<Project>> { Succeeded = false, StatusCode = result.StatusCode, Data = new List<Project>(), ErrorMessage = "Failed to retrieve projects." };
    }

    public async Task<ProjectResult<List<Project>>> GetProjectCardsAsync()
    {
        var result = await _projectRepository.GetAllAsync(
            selector: p => new Project
            {
                Id = p.Id,
                ProjectTitle = p.ProjectTitle,
                Description = p.Description,
                Client = new Client
                {
                    Id = p.Client.Id,
                    ClientName = p.Client.ClientName,
                },
                ImageUrl = p.Picture.ImageUrl,
                StatusName = p.StatusName,
                Budget = p.Budget,       
                EndDate = p.EndDate,
                StartDate = p.StartDate,
                ProjectMembers = p.ProjectMembers!.Select(member => new MemberUser
                {
                    Id = member.Member.Id,
                    FirstName = member.Member.FirstName ?? "Unknown",
                    LastName = member.Member.LastName ?? "Unknown",
                    Email = member.Member.Email ?? "Unknown",
                    PhoneNumber = member.Member.PhoneNumber ?? "Unknown",
                    ImageUrl = member.Member.Picture!.ImageUrl ?? "Unknown",
                    JobTitle = member.Member.JobTitle ?? "Unknown",
                }).ToList()
            });

        return result.Success && result.Data != null
           ? new ProjectResult<List<Project>> { Succeeded = true, StatusCode = 200, Data = result.Data.ToList() }
           : new ProjectResult<List<Project>> { Succeeded = false, StatusCode = result.StatusCode, Data = new List<Project>(), ErrorMessage = "Failed to retrieve projects." };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string value)
    {
        var result = await _projectRepository.GetAsync(
            filterBy: x => x.ProjectTitle.ToLower() == value.ToLower() || x.Description == value || x.StatusName.ToLower() == value.ToLower(),
            includes: null!);

        return result.Success && result.Data != null
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Data = ProjectFactory.CreateModelFromEntity(result.Data!) }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, ErrorMessage = "No project was found." };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(Guid id)
    {
        var result = await _projectRepository.GetProjectAsync(id);

        return result.Success && result.Data != null
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Data = ProjectFactory.CreateModelFromEntity(result.Data!) }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, ErrorMessage = "No project was found." };
    }


    public async Task<ProjectResult<bool>> UpdateAsync(ProjectDto formData)
    {
        if (formData == null)
            return new ProjectResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields are not completed.", Data = false };

        var entity = ProjectFactory.CreateEntityFromDto(formData);
        entity.Id = formData.Id!.Value;
        entity.ClientId = formData.ClientId!.Value;

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


    public async Task<ProjectResult<bool>> DeleteAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return new ProjectResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Invalid member provided." };

            await _projectRepository.BeginTransactionAsync();

            var project = await _projectRepository.GetProjectAsync(id);

            if (project.Data == null)
                return new ProjectResult<bool> { Succeeded = false, StatusCode = 404, ErrorMessage = "Member not found." };

            var result = await _projectRepository.DeleteAsync(project.Data);

            if (!result.Success)
                return new ProjectResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Unable to delete member.", Data = false };

            if (project.Data.Picture != null)
            {
                var pictureResult = await _pictureRepository.DeleteAsync(project.Data.Picture);
                if (!pictureResult.Success)
                    return new ProjectResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Unable to delete picture.", Data = false };
            }
            await _projectRepository.CommitTransactionAsync();

            return new ProjectResult<bool> { Succeeded = true, StatusCode = 200, Data = true };
        }
        catch (Exception ex)
        {
            var rollback = await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new ProjectResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to delete member: {ex.Message}", Data = false };
        }
    }
}

