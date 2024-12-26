using WebMarket.Models;

namespace WebMarket.Controllers
{
    public class ProductController : MyController<Product>
    {
        public ProductController(MarketContext context) : base(context, con => con.Products)
        {
        }
    }
}
