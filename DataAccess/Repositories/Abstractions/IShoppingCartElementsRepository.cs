using WebMarket.DataAccess.Models;

namespace WebMarket.DataAccess.Repositories.Abstractions
{
    public interface IShoppingCartElementsRepository : IBaseRepository<ShoppingCartElement>
    {
        Task<int> UpdateAmount(int elementId, int amount);
    }
}
