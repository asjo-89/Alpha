using Data.Models;
using System.Linq.Expressions;

namespace Data.Interfaces
{
    public interface IBaseRepository<TEntity, TModel> where TEntity : class
    {
        Task<RepositoryResult> BeginTransactionAsync();
        Task<RepositoryResult> CommitTransactionAsync();
        Task<RepositoryResult<bool>> CreateAsync(TEntity entity);
        Task<RepositoryResult<bool>> DeleteAsync(TEntity entity);
        Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> expression);
        Task<RepositoryResult<IEnumerable<TModel>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>> orderBy = null!, Expression<Func<TEntity, bool>> filterBy = null!, params Expression<Func<TEntity, object>>[] includes);
        Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescending = false, Expression<Func<TEntity, object>> orderBy = null!, Expression<Func<TEntity, bool>> filterBy = null!, params Expression<Func<TEntity, object>>[] includes) where TSelect : class, new();
        Task<RepositoryResult<TModel>> GetAsync(Expression<Func<TEntity, bool>> filterBy = null!, params Expression<Func<TEntity, object>>[] includes);
        Task<RepositoryResult> RollbackTransactionAsync();
        Task<RepositoryResult<bool>> UpdateAsync(TEntity entity);
    }
}