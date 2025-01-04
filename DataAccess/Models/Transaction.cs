namespace WebMarket.DataAccess.Models;

public partial class Transaction : DbEntry
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Amount { get; set; }

    public int Cost { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
