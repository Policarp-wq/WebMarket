using Microsoft.AspNetCore.Mvc;
using WebMarket.Contracts.Review;
using WebMarket.DataAccess.Models;
using WebMarket.Services;

namespace WebMarket.Controllers
{
    public class ReviewController : MyController<Review>
    {
        private readonly ReviewService _reviewService;
        public ReviewController(ReviewService reviewService) : base(reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductReviews(int productId)
        {
            return CreatedAtAction(nameof(GetProductReviews), await _reviewService.GetProductReviews(productId));
        }

        [HttpPost]
        public async Task<IActionResult> PublishReview([FromBody] ReviewAddInfo review)
        {
            if (review == null)
                return BadRequest("Body is null");
            return Ok(await _reviewService.Create(new Review()
            {
                UserId = review.UserId,
                ProductId = review.ProductId,
                ReviewContent = review.ReviewContent,
                SetRating = review.SetRating,
            }));
        }
    }
}
