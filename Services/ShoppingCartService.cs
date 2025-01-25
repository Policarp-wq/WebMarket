using StackExchange.Redis;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.Services
{
    public class ShoppingCartService : BaseService<ShoppingCartElement>
    {
        private readonly IShoppingCartElementsRepository _shoppingCartRepository;
        public ShoppingCartService(IShoppingCartElementsRepository shoppingCartRepository, IConnectionMultiplexer multiplexer)
            : base(shoppingCartRepository, multiplexer)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<int> UpdateAmount(int elementId, int amount)
        {
            return await _shoppingCartRepository.UpdateAmount(elementId, amount);
        }
    }
}
