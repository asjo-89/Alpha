using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class ProjectNoteRepository(AlphaDbContext context) : BaseRepository<ProjectNoteEntity, ProjectNote>(context), IProjectNoteRepository
{
}
