using Data.Contexts;
using Data.Interfaces;
using Data.Models;
using Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace Data.Repositories;

public class BaseRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel> where TEntity : class
{
    protected readonly AlphaDbContext _context;
    protected readonly DbSet<TEntity> _entity;

    public BaseRepository(AlphaDbContext context)
    {
        _context = context;
        _entity = _context.Set<TEntity>();
    }



    public async Task<RepositoryResult<bool>> CreateAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Success = false, StatusCode = 400, Error = "Entity must not be null." };

        try
        {
            var result = await _entity.AddAsync(entity);
            if (result != null)
            {
                var saved = await _context.SaveChangesAsync();
                return new RepositoryResult<bool> { Success = true, StatusCode = 201 };
            }
            return new RepositoryResult<bool> { Success = false, StatusCode = 500, Error = "Unable to add entity." };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<bool> { Success = false, StatusCode = 500, Error = $"Something wen wrong creating entity: {ex.Message}" };
        }
    }



    // Get all with specific order.
    public async Task<RepositoryResult<IEnumerable<TModel>>> GetAllAsync
        (
            bool orderByDescending = false,
            Expression<Func<TEntity, bool>> orderBy = null!,
            Expression<Func<TEntity, bool>> filterBy = null!,
            params Expression<Func<TEntity, bool>>[] includes
        )
    {
        IQueryable<TEntity> query = _entity;

        if (filterBy != null)
            query = query.Where(filterBy);

        if (includes != null && includes.Length != 0)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        if (orderBy != null)
        {
            query = orderByDescending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);
        }

        var entities = await query.ToListAsync();
        var result = entities.Select(entity => entity.MapTo<TModel>());

        return new RepositoryResult<IEnumerable<TModel>> { Success = true, StatusCode = 200, Data = result };
    }



    // Get all with specific order and based on specific data
    public async Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>
        (
            Expression<Func<TEntity, TSelect>> selector,
            bool orderByDescending = false,
            Expression<Func<TEntity, bool>> orderBy = null!,
            Expression<Func<TEntity, bool>> filterBy = null!,
            params Expression<Func<TEntity, bool>>[] includes
        )
    {
        IQueryable<TEntity> query = _entity;

        if (filterBy != null)
            query = query.Where(filterBy);

        if (includes != null && includes.Length != 0)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        if (orderBy != null)
        {
            query = orderByDescending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);
        }

        var entities = await query.Select(selector).ToListAsync();

        var result = entities.Select(entity => entity!.MapTo<TSelect>());
        return new RepositoryResult<IEnumerable<TSelect>> { Success = true, StatusCode = 200, Data = result };
    }



    public async Task<RepositoryResult<TModel>> GetAsync
        (
            Expression<Func<TEntity, bool>> filterBy = null!,
            params Expression<Func<TEntity, bool>>[] includes
        )
    {
        IQueryable<TEntity> query = _entity;

        if (includes != null && includes.Length != 0)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        var entity = await query.FirstOrDefaultAsync(filterBy);
        if (entity == null)
            return new RepositoryResult<TModel> { Success = false, StatusCode = 404, Error = "Entity could not be found." };

        var result = entity.MapTo<TModel>();
        return new RepositoryResult<TModel> { Success = true, StatusCode = 200, Data = result };
    }



    public async Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        bool result = await _entity.AnyAsync(expression);
        return !result
            ? new RepositoryResult<bool> { Success = false, StatusCode = 404, Error = "Entity was not found." }
            : new RepositoryResult<bool> { Success = true, StatusCode = 200 };
    }



    public async Task<RepositoryResult<bool>> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Success = false, StatusCode = 400, Error = "Entity must not be null." };

        try
        {
            var result = _entity.Update(entity);
            if (result != null)
            {
                var saved = await _context.SaveChangesAsync();
                return new RepositoryResult<bool> { Success = true, StatusCode = 200 };
            }
            return new RepositoryResult<bool> { Success = false, StatusCode = 500, Error = "Unable to update entity." };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<bool> { Success = false, StatusCode = 500, Error = $"Something wen wrong updating entity: {ex.Message}" };
        }
    }



    public async Task<RepositoryResult<bool>> DeleteAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Success = false, StatusCode = 400, Error = "Entity must not be null." };

        try
        {
            var result = _entity.Remove(entity);
            if (result != null)
            {
                var saved = await _context.SaveChangesAsync();
                return new RepositoryResult<bool> { Success = true, StatusCode = 200 };
            }
            return new RepositoryResult<bool> { Success = false, StatusCode = 500, Error = "Unable to delete entity." };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<bool> { Success = false, StatusCode = 500, Error = $"Something wen wrong deleting entity: {ex.Message}" };
        }
    }
}
