using StackExchange.Redis;
using WebMarket.Contracts.Review;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.Services
{
    public class ReviewService : BaseService<Review>
    {
        private IReviewsRepository _reviewsRepository;
        public ReviewService(IReviewsRepository reviewsRepository, IConnectionMultiplexer multiplexer) : base(reviewsRepository, multiplexer)
        {
            _reviewsRepository = reviewsRepository;
        }

        public async Task<List<ReviewPresentation>> GetProductReviews(int productId)
        {
            return await _reviewsRepository.GetProductReviews(productId);
        }
    }
}
