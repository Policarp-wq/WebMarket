using StackExchange.Redis;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.Services
{
    public class ProductService : BaseService<Product>
    {
        private readonly IProductsRepository _productRepository;
        public ProductService(IProductsRepository productRepository, IConnectionMultiplexer multiplexer) : base(productRepository, multiplexer)
        {
            _productRepository = productRepository;
        }
    }
}
