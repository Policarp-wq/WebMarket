using WebMarket.DataAccess.Models;

namespace WebMarket.DataAccess.Repositories.Abstractions
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<User?> GetUserByLogin(string login);
        Task<int> Update(int id, string? login, string? email, string? address);
    }
}