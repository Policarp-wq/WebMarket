using WebMarket.DataAccess.Models;

namespace WebMarket.DataAccess.Repositories.Abstractions
{
    public interface IBaseRepository<T> where T : DbEntry
    {
        Task<T> Create(T entity);
        Task<T?> GetById(int id);
        Task<List<T>> Index();
        Task<int> DeleteById(int id);
    }
}