using Data.Contexts;
using Data.Entities;
using Domain.Models;

namespace Data.Repositories;

public class ProjectNoteRepository(AlphaDbContext context) : BaseRepository<ProjectNoteEntity, ProjectNote>(context)
{
}
