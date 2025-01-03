using StackExchange.Redis;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    public class SpecialController : CRUDController<Special>
    {
        public SpecialController(MarketContext context, IConnectionMultiplexer multiplexer) : base(context, con => con.Specials, multiplexer)
        {
        }
    }
}
