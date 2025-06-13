using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGame.DB.Repositories.Services
{
    public class EntityRepository<TEntity, TContext> : IEntityRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly TContext _context;

        public EntityRepository(IDbContextFactory<TContext> db)
        {
            _context ??= db.CreateDbContext();
        }

        /// <summary>
        /// Retrieves the first entity matching the predicate.
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>Found entity or null</returns>
        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Retrieves all entities that satisfy the predicate.
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>List of entities</returns>
        public async Task<List<TEntity>> GetEntities(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }
    }
}
