namespace WebMarket.DataAccess.Models
{
    public class ShoppingCartElement : DbEntry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int ProductAmount { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}

