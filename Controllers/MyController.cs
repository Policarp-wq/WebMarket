using Microsoft.AspNetCore.Mvc;

namespace WebMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class MyController
    {
        protected 
        [HttpGet("{id}")] 
        public virtual Task<IActionResult> GetById(int id)
        {

        }
    }
}
