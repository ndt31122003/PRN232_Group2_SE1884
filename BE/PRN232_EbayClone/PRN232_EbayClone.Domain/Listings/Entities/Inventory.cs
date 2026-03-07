using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Inventory
{
    public int Id { get; set; }

    public Guid? Productid { get; set; }

    public int? Quantity { get; set; }

    public DateTime? Lastupdated { get; set; }

    public virtual Product? Product { get; set; }
}
