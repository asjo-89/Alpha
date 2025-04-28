using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResult<Project>> CreateAsync(ProjectDto formData);
        Task<ProjectResult<bool>> DeleteAsync(Guid id);
        Task<ProjectResult<Project>> GetProjectAsync(string value);
        Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
        Task<ProjectResult<Project>> GetProjectAsync(Guid id);
        Task<ProjectResult<bool>> UpdateAsync(ProjectDto formData);
        Task<ProjectResult<List<Project>>> GetProjectsWithDetailsAsync();
        Task<ProjectResult<List<Project>>> GetProjectCardsAsync();
    }
}