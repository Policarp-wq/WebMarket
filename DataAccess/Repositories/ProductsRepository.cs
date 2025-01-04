using Microsoft.EntityFrameworkCore;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.DataAccess.Repositories
{
    public class ProductsRepository : EntityRepository<Product>, IProductsRepository
    {
        public ProductsRepository(MarketContext context) : base(context, context => context.Products)
        {
        }

        public async override Task<Product> Create(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("Adding product is null!");
            var res = await _dbSet.AddAsync(product);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<List<Product>> GetPartition(int limit, int offset)
        {
            return await _dbSet
                .AsNoTracking()
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }
    }
}
