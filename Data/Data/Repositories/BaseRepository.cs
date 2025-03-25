using Data.Contexts;
using Data.Errors;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Data.Repositories;

public class BaseRepository<TEntity>(AlphaDbContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AlphaDbContext _context = context;
    protected readonly DbSet<TEntity> _entities = context.Set<TEntity>();

    public IDbContextTransaction? _transaction;

    #region Transactions
    public async Task<Result> BeginTransactionAsync()
    {
        Result result = new();

        if (_transaction != null)
            return new Result { Success = false, ErrorMessage = "A transaction is already started." };

        _transaction = await _context.Database.BeginTransactionAsync();
        return new Result { Success = true };
    }

    public async Task<Result> CommitTransactionAsync()
    {
        Result result = new();

        if (_transaction == null)
            return new Result { Success = false, ErrorMessage = "No transaction has been started." };

        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null!;

        return new Result { Success = true };
    }

    public async Task<Result> RollbackTransactionAsync()
    {
        Result result = new();

        if (_transaction == null)
            return new Result { Success = false, ErrorMessage = "There is no transaction to roll back." };

        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null!;

        return new Result { Success = true };
    }

    public async Task<Result> SaveChangesAsync()
    {
        if (_transaction == null)
            return new Result { Success = false, ErrorMessage = "There is no transaction to save." };

        await _context.SaveChangesAsync();

        return new Result { Success = true };    
    }
    #endregion



    #region CRUD
    public async Task<bool> CreateAsync(TEntity entity)
    {
        if (entity == null) return false;

        await _entities.AddAsync(entity);
        return true;
    }

    public async Task<ICollection<TEntity>> GetAllAsync()
    {
        var entities = await _entities.ToListAsync();
        return entities;
    }

    public async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression)
    {
        TEntity? entity = await _entities.FirstOrDefaultAsync(expression);
        return entity;
    }

    public bool UpdateAsync(TEntity entity)
    {
        if (entity == null) return false;

        _entities.Update(entity);
        return true;
    }

    public bool DeleteAsync(TEntity entity)
    {
        if (entity == null) return false;

        _entities.Remove(entity);
        return true;
    }
    #endregion
}
