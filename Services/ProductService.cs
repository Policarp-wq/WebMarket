using StackExchange.Redis;
using WebMarket.Contracts;
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

        public async Task<List<ProductSearchPreview>> SearchByKeyword(string keyword)
        {
            return await _productRepository.SearchByKeyword(keyword);
        }

        public async Task<List<ProductSearchPreview>> GetProductsByCategory(string category)
        {
            return await _productRepository.GetProductsByCategory(category);
        }
        public async Task<List<Product>> GetProductPartitionByCategory(int limit, int offset, int category)
        {
            return await _productRepository.GetProductPartitionByCategory(limit, offset, category);
        }
    }
}
