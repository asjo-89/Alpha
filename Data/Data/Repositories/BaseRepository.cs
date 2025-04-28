using Data.Contexts;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Data.Repositories;

public class BaseRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel> where TEntity : class where TModel : class, new()
{
    protected readonly AlphaDbContext _context;
    protected readonly DbSet<TEntity> _entity;

    private IDbContextTransaction _transaction = null!;

    public BaseRepository(AlphaDbContext context)
    {
        _context = context;
        _entity = _context.Set<TEntity>();
    }


    #region CRUD
        
    public async Task<RepositoryResult<bool>> CreateAsync(TEntity entity)
    {
        Console.WriteLine("BaseCreate");
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
    public async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync
        (
            bool orderByDescending = false,
            Expression<Func<TEntity, object>> orderBy = null!,
            Expression<Func<TEntity, bool>> filterBy = null!,
            params Expression<Func<TEntity, object>>[] includes
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

        return entities.Count != 0
            ? new RepositoryResult<IEnumerable<TEntity>> { Success = true, StatusCode = 200, Data = entities ?? [] }
            : new RepositoryResult<IEnumerable<TEntity>> { Success = false, StatusCode = 404, Error = "No entities found." };
    }

    // Get all with specific order and based on specific data
    public async Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect> 
        (
            Expression<Func<TEntity, TSelect>> selector,
            bool orderByDescending = false,
            Expression<Func<TEntity, object>> orderBy = null!,
            Expression<Func<TEntity, bool>> filterBy = null!,
            params Expression<Func<TEntity, object>>[] includes
        ) where TSelect : class, new()
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

        var result = await query.Select(selector).ToListAsync();

        //var result = entities.Select(entity => entity!.MapTo<TSelect>());
        return new RepositoryResult<IEnumerable<TSelect>> { Success = true, StatusCode = 200, Data = result };
    }



    // Get one with specific data
    public async Task<RepositoryResult<TEntity>> GetAsync
        (
            Expression<Func<TEntity, bool>> filterBy = null!,
            params Expression<Func<TEntity, object>>[] includes
        )
    {
        IQueryable<TEntity> query = _entity.AsNoTracking();

        if (includes != null && includes.Length != 0)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        var entity = await query.FirstOrDefaultAsync(filterBy);
        if (entity == null)
            return new RepositoryResult<TEntity> { Success = false, StatusCode = 404, Error = "Entity could not be found." };

        return new RepositoryResult<TEntity> { Success = true, StatusCode = 200, Data = entity };
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
    #endregion

    #region Transactions
    public async Task<RepositoryResult> BeginTransactionAsync()
    {
        if (_transaction != null)
            return new RepositoryResult { Success = false, StatusCode = 409, Error = "Another transaction is already started." };

        _transaction = await _context.Database.BeginTransactionAsync();
        return new RepositoryResult { Success = true, StatusCode = 200 };
    }

    public async Task<RepositoryResult> CommitTransactionAsync()
    {
        if (_transaction == null)
            return new RepositoryResult { Success = false, StatusCode = 400, Error = "No transaction has been started yet" };

        try
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;

            return new RepositoryResult { Success = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await _transaction.RollbackAsync();
            Console.WriteLine($"************\n{ex.Message}\n**************");
            _transaction = null!;

            return new RepositoryResult { Success = false, StatusCode = 500, Error = $"Failed to commit transaction: {ex.Message}" };
        }
    }

    public async Task<RepositoryResult> RollbackTransactionAsync()
    {
        if (_transaction == null)
            return new RepositoryResult { Success = false, StatusCode = 400, Error = "No transaction has been started yet" };

        try
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;

            return new RepositoryResult { Success = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"************\n{ex.Message}\n**************");
            _transaction = null!;

            return new RepositoryResult { Success = false, StatusCode = 500, Error = $"Failed to rollback transaction: {ex.Message}" };
        }
    }

    #endregion
}
