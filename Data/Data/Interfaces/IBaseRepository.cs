using Data.Errors;
using System.Linq.Expressions;

namespace Data.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<bool> CreateAsync(TEntity entity);
    Task<ICollection<TEntity>> GetAllAsync();
    Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression);
    bool UpdateAsync(TEntity entity);
    bool DeleteAsync(TEntity entity);



    Task<Result> BeginTransactionAsync();
    Task<Result> CommitTransactionAsync();
    Task<Result> RollbackTransactionAsync();
    Task<Result> SaveChangesAsync();
}
