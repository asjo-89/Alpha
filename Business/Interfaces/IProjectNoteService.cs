using Business.Models;
using Domain.Dtos;

namespace Business.Interfaces
{
    public interface IProjectNoteService
    {
        Task<ProjectNoteResult> CreateAsync(ProjectNoteFormData formData);
        Task<ProjectNoteResult> DeleteAsync(ProjectNoteFormData formData);
        Task<ProjectNoteResult> UpdateAsync(ProjectNoteFormData formData);
    }
}