﻿using System;
using System.Collections.Generic;

namespace WebMarket.Models;

public partial class Special : DbEntry
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}