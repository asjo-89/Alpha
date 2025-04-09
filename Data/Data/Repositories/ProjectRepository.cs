using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class ProjectRepository(AlphaDbContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{
}
