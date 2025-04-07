using Business.Dtos;
using Business.Models;
using System.Linq.Expressions;

namespace Business.Interfaces;

public interface IProjectNotesService
{
    Task<bool> CreateAsync(ProjectNoteRegForm form);
    Task<IEnumerable<ProjectNotesModel>> GetAllAsync();
    Task<ProjectNotesModel> GetOneAsync(Expression<Func<ProjectNotesModel, bool>> expression);
    Task<bool> UpdateAsync(ProjectNotesModel model);
    Task<bool> DeleteAsync(ProjectNotesModel model);
}
