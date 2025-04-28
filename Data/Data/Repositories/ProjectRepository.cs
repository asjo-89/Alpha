using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository(AlphaDbContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{
    public async Task<RepositoryResult<ProjectEntity>> CreateProjectAsync(ProjectEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<ProjectEntity> { Success = false, StatusCode = 400, Error = "Entity must not be null." };

        try
        {
            var result = await _entity.AddAsync(entity);
            if (result != null)
            {
                var saved = await _context.SaveChangesAsync();
                return new RepositoryResult<ProjectEntity> { Success = true, StatusCode = 201, Data = result.Entity };
            }
            return new RepositoryResult<ProjectEntity> { Success = false, StatusCode = 500, Error = "Unable to add entity." };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<ProjectEntity> { Success = false, StatusCode = 500, Error = $"Something wen wrong creating entity: {ex.Message}" };
        }
    }

    public async Task<RepositoryResult<ProjectEntity>> GetProjectAsync(Guid id)
    {
        var entity = await _context.Projects
            .Include(x => x.Picture)
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity != null 
            ? new RepositoryResult<ProjectEntity> { Success = true, StatusCode = 200, Data = entity }
            : new RepositoryResult<ProjectEntity> { Success = false, StatusCode = 400, Error = "Failed to get project." };
    }
}
