using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class ProjectMemberRepository(AlphaDbContext context) : BaseRepository<ProjectMemberEntity, ProjectMember>(context), IProjectMemberRepository
{
    public async Task<bool> AddAsync(ProjectMemberEntity entity)
    {
        if (entity == null)
            return false;

        try
        {
            _context.ProjectMembers.AddRange(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add ProjectMembers: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(IEnumerable<ProjectMemberEntity> entities)
    {
        if (entities == null)
            return false;

        try
        {
            var entitiesToRemove = entities.Select(entity => new ProjectMemberEntity
            {
                ProjectId = entity.ProjectId,
                MemberId = entity.MemberId
            }).ToList();

            _context.ProjectMembers.RemoveRange(entitiesToRemove);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to remove ProjectMembers: {ex.Message}");
            return false;
        }
    }
}
