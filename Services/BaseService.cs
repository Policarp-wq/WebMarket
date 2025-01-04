using StackExchange.Redis;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.Services
{
    public abstract class BaseService<T> where T : DbEntry
    {
        protected readonly IDatabase _redis;
        protected readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> baseRepository, IConnectionMultiplexer multiplexer)
        {
            _redis = multiplexer.GetDatabase();
            _repository = baseRepository;
        }

        public virtual async Task<List<T>> GetIndex()
        {
            return await _repository.Index();
        }

        public virtual async Task<T> Create(T entity)
        {
            return await _repository.Create(entity);
        }

        public virtual async Task<T?> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public virtual async Task<int> DeleteById(int id)
        {
            return await _repository.DeleteById(id);
        }

    }
}
