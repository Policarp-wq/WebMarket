using StackExchange.Redis;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.Services
{
    public class UserService : BaseService<User>
    {
        private readonly IUsersRepository _userRepository;

        public UserService(IUsersRepository userRepository, IConnectionMultiplexer multiplexer) : base(userRepository, multiplexer)
        {
            _userRepository = userRepository;
        }

        public async Task<User> RegisterUser(string login, string password, string email, string? address)
        {
            return await _userRepository.Create(new User()
            {
                Login = login,
                PasswordHash = HashPassword(password),
                Email = email,
                Address = address
            });
        }
        //TODO!
        private string HashPassword(string password)
        {
            return password;
        }

        public async Task<User?> GetByLogin(string login)
        {
            return await _userRepository.GetUserByLogin(login);
        }

        public async Task<int> Update(int id, string? login, string? email, string? address)
        {
            return await _userRepository.Update(id, login, email, address);
        }
    }
}
