using StackExchange.Redis;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    public class ProductController : CRUDController<Product>
    {
        public ProductController(MarketContext context, IConnectionMultiplexer multiplexer)
            : base(context, con => con.Products, multiplexer)
        {
        }
    }
}
