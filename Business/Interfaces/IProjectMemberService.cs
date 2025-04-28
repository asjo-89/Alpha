using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectMemberService
    {
        Task<bool> AddAsync(ProjectMemberDto dto);
        Task<bool> DeleteAsync(IEnumerable<ProjectMember> projectMembers);
        Task<List<ProjectMember>> ExistingAsync(ProjectDto dto);
        Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(Guid id);
    }
}