using WebMarket.DataAccess.Models;

namespace WebMarket.DataAccess.Repositories.Abstractions
{
    public interface IPartitionRepository<T> where T : DbEntry
    {
        public Task<List<T>> GetPartition(int limit, int offset);
    }
}
