using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebMarket.Contracts;
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

        public async Task<List<ProductSearchPreview>> SearchByKeyword(string keyword)
        {
            if (keyword.Length == 0)
                return [];
            keyword = keyword.ToLower();
            var keywordParam = new NpgsqlParameter("keywordParam", keyword);
            return await _context.Database.SqlQuery<ProductSearchPreview>($"SELECT * FROM select_products_matching_keyword({keywordParam});")
                .ToListAsync();
        }

        public async Task<List<ProductSearchPreview>> GetProductsByCategory(string category)
        {
            return await _dbSet
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.Category != null && p.Category.Tag.Equals(category))
                .Select(p => new ProductSearchPreview(p.Id, p.Name, category, p.Rating))
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductPartitionByCategory(int limit, int offset, int categoryId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.CategoryId != null && p.CategoryId == categoryId)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }
    }
}
