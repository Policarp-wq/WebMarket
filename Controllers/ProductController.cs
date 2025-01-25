using Microsoft.AspNetCore.Mvc;
using WebMarket.Contracts;
using WebMarket.DataAccess.Models;
using WebMarket.Services;

namespace WebMarket.Controllers
{
    public class ProductController : MyController<Product>
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService) : base(productService)
        {
            _productService = productService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductInfo productInfo)
        {
            if (productInfo == null)
                return BadRequest("Body is null");
            var res = await _productService.Create(new Product()
            {
                Name = productInfo.Name,
                CategoryId = productInfo.CategoryId,
                Price = productInfo.Price,
                Description = productInfo.Description,
                Image = productInfo.Image,
                Rating = productInfo.Rating,

            });
            return CreatedAtAction(nameof(Create), res);
        }

        [HttpGet("{keyword}")]
        public async Task<IActionResult> SearchbyKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return BadRequest("Keyword was empty");
            var res = await _productService.SearchByKeyword(keyword);
            return Ok(res);
        }
        [HttpGet("{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var res = await _productService.GetProductsByCategory(category);
            return Ok(res);
        }
    }
}
