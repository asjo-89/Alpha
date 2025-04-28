using Data.Entities;
using Domain.Models;

namespace Data.Interfaces
{
    public interface IProjectMemberRepository : IBaseRepository<ProjectMemberEntity, ProjectMember>
    {
        Task<bool> AddAsync(ProjectMemberEntity entity);
        Task<bool> DeleteAsync(IEnumerable<ProjectMemberEntity> entities);
    }
}