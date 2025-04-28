using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectNoteService
    {
        Task<ProjectNoteResult> CreateAsync(ProjectNoteDto formData);
        Task<ProjectNoteResult<IEnumerable<ProjectNote>>> GetNotesAsync();
        Task<ProjectNoteResult<ProjectNote>> GetNoteAsync(Guid projectId);
        Task<ProjectNoteResult> DeleteAsync(ProjectNoteDto formData);
        Task<ProjectNoteResult> UpdateAsync(ProjectNoteDto formData);
    }
}