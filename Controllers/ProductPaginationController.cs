using StackExchange.Redis;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.Controllers
{
    public class ProductPaginationController : PaginationController<Product>
    {
        public ProductPaginationController(IProductsRepository partitionRepository,
            IConnectionMultiplexer multiplexer) : base(partitionRepository, multiplexer)
        {
        }
    }
}
