using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    public class UserController : CRUDController<User>
    {
        public UserController(MarketContext context, IConnectionMultiplexer multiplexer) : base(context, con => con.Users, multiplexer)
        {
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] User user)
        {
            if (user == null)
                return new BadRequestObjectResult("User is empty");
            var authed = await _dbSet.FirstOrDefaultAsync(u => u.Login.Equals(user.Login) && u.PasswordHash.Equals(user.PasswordHash));
            if (authed == null)
                return new UnauthorizedObjectResult("No user with this password and login");
            return new OkObjectResult(authed);
        }
    }
}
