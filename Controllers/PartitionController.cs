using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WebMarket.Models;
using WebMarket.Services;

namespace WebMarket.Controllers
{
    public abstract class PartitionController<T> : MyController<T> where T : DbEntry
    {
        protected Dictionary<string, int> _offsets;
        protected string _table;
        protected PartitionController(MarketContext context, Func<MarketContext, DbSet<T>> getDbSet,
            IConnectionMultiplexer multiplexer, string table) : base(context, getDbSet, multiplexer)
        {
            _offsets = new Dictionary<string, int>();
            _table = table;
        }

        protected async Task<bool> AddTokenToMemory(string token)
        {
            return await _redis.StringSetAsync(token, 0, TimeSpan.FromMinutes(60));
        }

        protected async Task<int> GetOffset(string token)
        {
            var res = await _redis.StringGetAsync(token);
            if (res.TryParse(out int offset))
                return offset;
            //exception!
            return -1;
        }

        protected async void IncrementOffset(string token, int add)
        {
            var res = await _redis.StringIncrementAsync(token, add);
        }

        [HttpGet]
        public async Task<IActionResult> GeneratePersonalSequenceToken()
        {
            var token = TokenGenerator.GetToken(32);
            bool ok = await AddTokenToMemory(token);
            if (!ok)
                return new BadRequestObjectResult("Failed adding token");
            return new ObjectResult(token);
        }

        [HttpGet("{count}")]
        public async Task<IActionResult> GetPartition([FromHeader(Name = "x-partition-token")] string? token, int count)
        {
            if (count == 0)
                return new ObjectResult(null);
            if (token == null || token.Length == 0)
                return new UnauthorizedObjectResult("No partition token header. Include x-partition-token!");
            int offset = await GetOffset(token);
            if (offset == -1)
                return new NotFoundObjectResult($"Didn't find token: {token}");
            //!!!
            var res = await _dbSet.FromSqlRaw($"SELECT * FROM \"{_table}\" LIMIT {count} OFFSET {offset};").ToListAsync();
            IncrementOffset(token, count);
            // count ?
            return new ObjectResult(res);
        }
    }
}
