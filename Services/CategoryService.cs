using StackExchange.Redis;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.Services
{
    public class CategoryService : BaseService<Category>
    {
        private readonly ICategoriesRepository _categoryRepository;
        public CategoryService(ICategoriesRepository categoryRepository, IConnectionMultiplexer multiplexer) : base(categoryRepository, multiplexer)
        {
            _categoryRepository = categoryRepository;
        }
    }
}
