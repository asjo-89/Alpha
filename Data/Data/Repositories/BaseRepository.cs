using Data.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Data.Repositories;

public class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context = context;
    protected readonly DbSet<TEntity> _entities = context.Set<TEntity>();

    public virtual async Task AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        IEnumerable<TEntity> entities = await _entities.ToListAsync();
        return entities;
    }

    public virtual async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _entities.FirstOrDefaultAsync(expression);
    }

    public virtual Task<TEntity> UpdateAsync(TEntity entity)
    {
        TEntity updatedEntity = _entities.Update(entity).Entity;
        return Task.FromResult(updatedEntity);
    }

    public virtual Task DeleteAsync(TEntity entity)
    {
        _entities.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        var result = await _context.SaveChangesAsync();
        return result;
    }
}
