namespace WebMarket.Models;

public partial class Product : DbEntry
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Price { get; set; }

    public string? Image { get; set; }

    public short? Rating { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Special> Specials { get; set; } = new List<Special>();

    public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
