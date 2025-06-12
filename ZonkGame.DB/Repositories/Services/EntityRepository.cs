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

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<List<TEntity>> GetEntities(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }
    }
}
