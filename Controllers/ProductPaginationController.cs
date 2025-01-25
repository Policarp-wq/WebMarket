using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;
using WebMarket.SupportTools;

namespace WebMarket.Controllers
{
    public class ProductPaginationController : PaginationController<Product>
    {
        private IProductsRepository _productsRepository;
        public ProductPaginationController(IProductsRepository partitionRepository,
            IConnectionMultiplexer multiplexer) : base(partitionRepository, multiplexer)
        {
            _productsRepository = partitionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonalSequenceCategoryToken()
        {
            var token = TokenGenerator.GetToken(_tokenLength);
            bool ok = await AddTokenToMemory(token);
            if (!ok)
                return new BadRequestObjectResult("Failed adding token");
            return new ObjectResult(token);
        }

        [HttpGet]
        public async Task<IActionResult> GetPageByCategory([FromHeader(Name = _paginationToken)] string? token, [FromQuery] int count, [FromQuery] int categoryId)
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
                var res = await _productsRepository.GetProductPartitionByCategory(count, offset, categoryId);
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
