using StackExchange.Redis;
using WebMarket.Authorization.JWT;
using WebMarket.Contracts;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;
using WebMarket.SupportTools;

namespace WebMarket.Services
{
    public class UserService : BaseService<User>
    {
        private readonly IUsersRepository _userRepository;
        private readonly JWTProvider _jwtProvider;

        public UserService(IUsersRepository userRepository, JWTProvider provider, IConnectionMultiplexer multiplexer) : base(userRepository, multiplexer)
        {
            _userRepository = userRepository;
            _jwtProvider = provider;
        }

        public async Task<AuthedUser> RegisterUser(string login, string password, string email, string? address)
        {
            var user = await _userRepository.Create(new User()
            {
                Login = login,
                PasswordHash = HashPassword(password),
                Email = email,
                Address = address
            });
            var token = _jwtProvider.GenerateToken(user);
            return new AuthedUser(user, token);
        }
        private static string HashPassword(string password)
        {
            return PasswordHasher.HashPassword(password);
        }

        public async Task<AuthedUser> Login(string login, string password)
        {
            var user = await _userRepository.GetUserByLogin(login);
            if (user == null)
                throw new Exception($"No user with given login {login}");
            if (!PasswordHasher.Verify(password, user.PasswordHash))
                throw new Exception("Wrong password");
            var token = _jwtProvider.GenerateToken(user);
            return new AuthedUser(user, token);
        }

        public async Task<int> Update(int id, string? login, string? email, string? address)
        {
            return await _userRepository.Update(id, login, email, address);
        }

        public async Task<List<ShoppingCartElementPresentation>> GetUserShoppingCart(int id)
        {
            return await _userRepository.GetShoppingCartElements(id);
        }
    }
}
