using Business.Dtos;
using Business.Models;
using System.Linq.Expressions;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<bool> CreateAsync(ProjectRegForm form);
    Task<IEnumerable<ProjectModel>> GetAllAsync();
    Task<ProjectModel> GetOneAsync(Expression<Func<ProjectModel, bool>> expression);
    Task<bool> UpdateAsync(ProjectModel model);
    Task<bool> DeleteAsync(ProjectModel model);
}
