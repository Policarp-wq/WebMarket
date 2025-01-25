namespace WebMarket.DataAccess.Models;

public partial class Product : DbEntry
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double Price { get; set; }

    public string? Image { get; set; }

    public double Rating { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Special> Specials { get; set; } = new List<Special>();

    public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual ICollection<ShoppingCartElement> ShoppingCartElements { get; set; } = new List<ShoppingCartElement>();
}
