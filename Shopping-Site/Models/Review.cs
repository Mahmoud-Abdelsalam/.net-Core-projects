using Microsoft.AspNetCore.Identity;

namespace ShoppingSite.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserReview { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public IdentityUser User { get; set; }
    }
}