namespace ShoppingSite.Models.Interfaces
{
    public interface IReviewRepository
    {
        Review AddReviewToProduct(string review, int productId, string userId);
    }
}