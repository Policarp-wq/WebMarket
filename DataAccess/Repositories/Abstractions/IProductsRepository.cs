using WebMarket.Contracts;
using WebMarket.DataAccess.Models;

namespace WebMarket.DataAccess.Repositories.Abstractions
{
    public interface IProductsRepository : IBaseRepository<Product>, IPartitionRepository<Product>
    {
        Task<List<ProductSearchPreview>> SearchByKeyword(string keyword);
        Task<List<ProductSearchPreview>> GetProductsByCategory(string category);
        Task<List<Product>> GetProductPartitionByCategory(int limit, int offset, int category);
    }
}
