using Microsoft.EntityFrameworkCore;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.DataAccess.Repositories
{
    public class ShoppingCartElementsRepository : EntityRepository<ShoppingCartElement>, IShoppingCartElementsRepository
    {
        public ShoppingCartElementsRepository(MarketContext context) : base(context, context => context.ShoppingCartElements)
        {
        }

        public override async Task<ShoppingCartElement> Create(ShoppingCartElement entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Adding cart element is null!");
            var res = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<int> UpdateAmount(int elementId, int amount)
        {
            if (amount < 0)
                return 0;
            return await _dbSet.Where(el => el.Id == elementId)
                .ExecuteUpdateAsync(el => el
                .SetProperty(el => el.ProductAmount, amount));
        }
    }
}
