using Microsoft.EntityFrameworkCore;
using WebMarket.Contracts.Review;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.DataAccess.Repositories
{
    public class ReviewsRepository : EntityRepository<Review>, IReviewsRepository
    {
        public ReviewsRepository(MarketContext context) : base(context, context => context.Reviews)
        {
        }

        public async override Task<Review> Create(Review entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var res = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<List<ReviewPresentation>> GetProductReviews(int productId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .Include(x => x.User)
                .Select(x => new ReviewPresentation(x.Id, x.User.Email, x.ReviewContent, x.SetRating, x.CreatedAt.GetValueOrDefault()))
                .ToListAsync();
        }
    }
}
