using WebMarket.DataAccess.Models;

namespace WebMarket.DataAccess.Repositories.Abstractions
{
    public interface IProductsRepository : IBaseRepository<Product>, IPartitionRepository<Product>
    {
    }
}
