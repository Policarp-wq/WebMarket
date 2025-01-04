using Microsoft.AspNetCore.Mvc;
using WebMarket.Contracts;
using WebMarket.DataAccess.Models;
using WebMarket.Services;

namespace WebMarket.Controllers
{
    public class CategoryController : MyController<Category>
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService) : base(categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryInfo categoryInfo)
        {
            if (categoryInfo == null)
                return BadRequest("Body is null");
            var res = await _categoryService.Create(new Category()
            {
                Tag = categoryInfo.Tag
            });
            return CreatedAtAction(nameof(Create), res);
        }
    }
}
