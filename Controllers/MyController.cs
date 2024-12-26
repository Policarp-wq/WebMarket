using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMarket.Models;
using WebMarket.Services;

namespace WebMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class MyController<T> where T : DbEntry
    {
        protected readonly MarketContext _dbContext;
        protected readonly Func<MarketContext, DbSet<T>> _getDbSet;
        //weird
        protected DbSet<T> _dbSet => _getDbSet(_dbContext);

        protected MyController(MarketContext context, Func<MarketContext, DbSet<T>> getDbSet)
        {
            _dbContext = context;
            _getDbSet = getDbSet;
        }
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
            return new CreatedAtActionResult(nameof(GetById), this.GetType().Name.Replace("Controller", ""), new { id = entity.Id }, entity);
        }
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (id < 0)
                return new BadRequestObjectResult($"Id: {id} < 0!");
            var entry = await _dbSet.FindAsync(id);
            if(entry == null) return new NotFoundResult();
            return new OkResult();
        }
    }
}
