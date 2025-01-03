using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class MyController<T> where T : DbEntry
    {
        protected readonly MarketContext _dbContext;
        private readonly Func<MarketContext, DbSet<T>> _getDbSet;
        protected readonly IDatabase _redis;
        //weird
        protected DbSet<T> _dbSet => _getDbSet(_dbContext);

        protected MyController(MarketContext context, Func<MarketContext, DbSet<T>> getDbSet, IConnectionMultiplexer multiplexer)
        {
            _dbContext = context;
            _getDbSet = getDbSet;
            _redis = multiplexer.GetDatabase();
        }

    }
}
