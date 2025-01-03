using StackExchange.Redis;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    public class ProductPartitionController : PartitionController<Product>
    {
        public ProductPartitionController(MarketContext context, IConnectionMultiplexer multiplexer)
            : base(context, context => context.Products, multiplexer, "products")
        {
        }
    }
}
