using System.Linq.Expressions;

namespace ZonkGame.DB.Repositories.Interfaces
{
    public interface IEntityRepository<TEntity, TContext>
    {
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetEntities(Expression<Func<TEntity, bool>> predicate);
    }
}
