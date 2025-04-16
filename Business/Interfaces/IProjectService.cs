using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResult<bool>> CreateAsync(ProjectFormData formData);
        Task<ProjectResult<bool>> DeleteAsync(ProjectFormData formData);
        Task<ProjectResult<Project>> GetMemberUserAsync(string value);
        Task<ProjectResult<IEnumerable<Project>>> GetMemberUsersAsync();
        Task<ProjectResult<bool>> UpdateAsync(ProjectFormData formData);
    }
}