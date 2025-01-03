namespace WebMarket.Models;

public partial class Storage : DbEntry
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? Amount { get; set; }

    public virtual Product? Product { get; set; }
}
