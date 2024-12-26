using System;
using System.Collections.Generic;

namespace WebMarket.Models;

public partial class User : DbEntry
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Address { get; set; }

    public decimal? Wallet { get; set; }
}
