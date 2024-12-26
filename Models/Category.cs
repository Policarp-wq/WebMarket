using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace WebMarket.Models;

public partial class Category : DbEntry
{
    public int Id { get; set; }

    public string Tag { get; set; } = null!;
    [ValidateNever]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
