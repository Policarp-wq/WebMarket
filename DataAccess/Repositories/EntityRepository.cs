using Microsoft.EntityFrameworkCore;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.DataAccess.Repositories
{
    public abstract class EntityRepository<T> : IBaseRepository<T> where T : DbEntry
    {
        protected readonly MarketContext _context;
        protected readonly DbSet<T> _dbSet;

        protected EntityRepository(MarketContext context, Func<MarketContext, DbSet<T>> getDbSet)
        {
            _context = context;
            _dbSet = getDbSet(_context);
        }

        public async virtual Task<List<T>> Index()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async virtual Task<T?> GetById(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async virtual Task<int> DeleteById(int id)
        {
            int affected = await _dbSet
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync();
            return affected > 0 ? id : -1;
        }
        public abstract Task<T> Create(T entity);
    }
}
