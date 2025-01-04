using Microsoft.AspNetCore.Mvc;
using WebMarket.DataAccess.Models;
using WebMarket.Services;

namespace WebMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class MyController<T> : ControllerBase where T : DbEntry
    {
        private readonly BaseService<T> _baseService;
        protected MyController(BaseService<T> baseService)
        {
            _baseService = baseService;
        }
        [HttpGet]
        public virtual async Task<ActionResult> Index()
        {
            return new OkObjectResult(await _baseService.GetIndex());
        }
        [HttpGet("{id}")]
        public virtual async Task<ActionResult> GetById(int id)
        {
            var res = await _baseService.GetById(id);
            if (res == null)
                return new NotFoundObjectResult($"Not found with id: {id}");
            return new OkObjectResult(res);
        }
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteById(int id)
        {
            var affected = await _baseService.DeleteById(id);
            if (affected > 0)
                return new OkObjectResult("Deleted");
            return new NotFoundObjectResult($"Not found with id: {id}");
        }



    }
}
