using Data.Entities;
using Domain.Models;

namespace Data.Interfaces;

public interface IProjectNoteRepository : IBaseRepository<ProjectNoteEntity, ProjectNote>
{
}
