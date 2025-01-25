using WebMarket.Contracts.Review;
using WebMarket.DataAccess.Models;

namespace WebMarket.DataAccess.Repositories.Abstractions
{
    public interface IReviewsRepository : IBaseRepository<Review>
    {
        public Task<List<ReviewPresentation>> GetProductReviews(int productId);
    }
}
