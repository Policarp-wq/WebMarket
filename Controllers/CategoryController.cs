using StackExchange.Redis;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    public class CategoryController : CRUDController<Category>
    {
        public CategoryController(MarketContext context, IConnectionMultiplexer multiplexer) : base(context, con => con.Categories, multiplexer)
        {
        }
    }
}
