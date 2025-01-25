namespace WebMarket.DataAccess.Models
{
    public class Review : DbEntry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string ReviewContent { get; set; } = null!;
        public short SetRating { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
