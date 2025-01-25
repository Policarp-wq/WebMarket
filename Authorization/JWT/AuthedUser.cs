using WebMarket.DataAccess.Models;

namespace WebMarket.Authorization.JWT
{
    public record AuthedUser(User User, string Token);
}
