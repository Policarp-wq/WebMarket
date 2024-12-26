using Microsoft.EntityFrameworkCore;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    public class UserController : MyController<User>
    {
        public UserController(MarketContext context) : base(context, con => con.Users)
        {
        }
    }
}
