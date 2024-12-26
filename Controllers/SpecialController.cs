using Microsoft.EntityFrameworkCore;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    public class SpecialController : MyController<Special>
    {
        public SpecialController(MarketContext context) : base(context, con => con.Specials)
        {
        }
    }
}
