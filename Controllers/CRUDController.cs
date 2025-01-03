using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WebMarket.Authorization;
using WebMarket.Models;

namespace WebMarket.Controllers
{
    public abstract class CRUDController<T> : MyController<T> where T : DbEntry
    {
        protected CRUDController(MarketContext context, Func<MarketContext, DbSet<T>> getDbSet, IConnectionMultiplexer multiplexer)
            : base(context, getDbSet, multiplexer)
        {
        }

        [ServiceFilter(typeof(ApiAuthFilter))]
        [HttpGet]
        public virtual async Task<IActionResult> Index()
        {
            return new ObjectResult(await _dbSet.ToListAsync());
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            //untrack?
            var entry = await _dbSet.FindAsync(id);
            if (entry == null)
                return new NotFoundResult();
            return new OkObjectResult(entry);
        }
        [ServiceFilter(typeof(ApiAuthFilter))]
        [HttpPost]
        public virtual async Task<IActionResult> Add([FromBody] T entity)
        {
            //db error not 500
            if (entity == null)
                return new BadRequestObjectResult("Body is null");
            if (entity.Id != 0 && await _dbSet.FindAsync(entity.Id) != null)
                return new BadRequestObjectResult("Entiuty with this id already exists");
            var res = await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(GetById), this.GetType().Name.Replace("Controller", ""), new { id = res.Entity.Id }, entity);
        }
        [ServiceFilter(typeof(ApiAuthFilter))]
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (id < 0)
                return new BadRequestObjectResult($"Id: {id} < 0!");
            var entry = await _dbSet.FindAsync(id);
            if (entry == null) return new NotFoundResult();
            return new OkResult();
        }
    }
}
