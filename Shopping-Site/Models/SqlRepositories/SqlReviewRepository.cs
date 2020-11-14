using ShoppingSite.Models.Interfaces;

namespace ShoppingSite.Models.SqlRepositories
{
    public class SqlReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public SqlReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public Review AddReviewToProduct(string review, int producIdt, string userId)
        {
            Review userreview = new Review()
            {
                UserId = userId,
                UserReview = review,
                ProductId = producIdt
            };
            _context.Reviews.Add(userreview);
            _context.SaveChanges();
            return userreview;
        }
    }
}