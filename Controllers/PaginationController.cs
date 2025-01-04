using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;
using WebMarket.SupportTools;

namespace WebMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class PaginationController<T> : ControllerBase where T : DbEntry
    {
        protected const string _paginationToken = "x-pagination-token";
        protected Dictionary<string, int> _offsets;
        protected readonly IDatabase _redis;
        protected readonly IPartitionRepository<T> _partitionRepository;
        private const int _tokenLength = 32;
        private const int _tokenTime = 60;
        protected PaginationController(IPartitionRepository<T> partitionRepository,
            IConnectionMultiplexer multiplexer)
        {
            _partitionRepository = partitionRepository;
            _offsets = new Dictionary<string, int>();
            _redis = multiplexer.GetDatabase();
        }

        protected async Task<bool> AddTokenToMemory(string token)
        {
            return await _redis.StringSetAsync(token, 0, TimeSpan.FromMinutes(_tokenTime));
        }

        protected async Task<int> GetOffset(string token)
        {
            var res = await _redis.StringGetAsync(token);
            if (res == RedisValue.Null)
                throw new KeyNotFoundException(token);
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
        public async Task<IActionResult> GetPersonalSequenceToken()
        {
            var token = TokenGenerator.GetToken(_tokenLength);
            bool ok = await AddTokenToMemory(token);
            if (!ok)
                return new BadRequestObjectResult("Failed adding token");
            return new ObjectResult(token);
        }

        [HttpGet("{count}")]
        public async Task<IActionResult> GetPage([FromHeader(Name = _paginationToken)] string? token, int count)
        {
            if (count <= 0)
                return new ObjectResult(null);
            if (token == null || token.Length == 0)
                return new UnauthorizedObjectResult($"No partition token header. Include {_paginationToken}");
            try
            {
                int offset = await GetOffset(token);
                if (offset == -1)
                    return new NotFoundObjectResult($"Count is out of range!");
                var res = await _partitionRepository.GetPartition(count, offset);
                IncrementOffset(token, count);
                return new ObjectResult(res);
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundObjectResult($"Didn't find token: {token}");
            }

        }
    }
}
