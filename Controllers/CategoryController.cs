using Microsoft.EntityFrameworkCore;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    public class CategoryController : MyController<Category>
    {
        public CategoryController(MarketContext context) : base(context, con => con.Categories)
        {
        }
    }
}
