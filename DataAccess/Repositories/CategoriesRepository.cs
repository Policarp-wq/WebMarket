using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.DataAccess.Repositories
{
    public class CategoriesRepository : EntityRepository<Category>, ICategoriesRepository
    {
        public CategoriesRepository(MarketContext context) : base(context, context => context.Categories)
        {
        }

        public async override Task<Category> Create(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("Adding category is null!");
            var res = await _dbSet.AddAsync(category);
            await _context.SaveChangesAsync();
            return res.Entity;
        }
    }
}
