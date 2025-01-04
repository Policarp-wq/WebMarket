namespace WebMarket.DataAccess.Models;

public partial class Category : DbEntry
{
    public int Id { get; set; }

    public string Tag { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
