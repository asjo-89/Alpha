using Data.Contexts;
using Data.Errors;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Data.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly AlphaDbContext _context;

    public IDbContextTransaction? _transaction;


    public BaseRepository(AlphaDbContext context)
    {
        _context = context;
    }

    #region Transactions
    public async Task<Result> BeginTransactionAsync()
    {
        Result result = new();

        if (_transaction != null)
        {
            result.Success = false;
            result.ErrorMessage = "Transaction already started.";
            return result;
        }

        try
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Failed to start transaction: {ex.Message}";
            return result;
        }
    }

    public async Task<Result> CommitTransactionAsync()
    {
        Result result = new();

        if (_transaction == null)
        {
            result.Success = false;
            result.ErrorMessage = "No transaction has been started.";
            return result;
        }

        try
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
            result.Success = true;
            return result;
        }       
        catch (Exception ex)
        {            
            await _transaction.RollbackAsync();
            result.Success= false;
            result.ErrorMessage = $"Failed to commit transaction: {ex.Message}";
            return result;
        }
    }

    public async Task<Result> RollbackTransactionAsync()
    {
        Result result = new();

        if (_transaction == null)
        {
            result.Success = false;
            result.ErrorMessage = "There is no transaction to roll back.";
            return result;
        }
         
        try
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;

            result.Success = true;
            return result;
        }
        catch (Exception ex) 
        {
            result.Success = false;
            result.ErrorMessage = $"Rollback failed: {ex.Message}";
            return result;
        }
    }

    public async Task<Result> SaveChangesAsync()
    {
        Result result = new();
        if (_transaction == null)
        {
            result.Success = false;
            result.ErrorMessage = "There is no transaction to save.";
            return result;
        }
        try
        {
            await _context.SaveChangesAsync();
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            await _transaction.RollbackAsync();
            result.Success = false;
            result.ErrorMessage = $"Failed to save changes to the database: {ex.Message}";
            return result;
        }
    }
    #endregion



    #region CRUD
    public Task<bool> CreateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
    #endregion
}
