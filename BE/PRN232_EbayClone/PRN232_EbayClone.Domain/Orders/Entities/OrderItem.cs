using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Orderitem
{
    public Guid Id { get; set; }

    public Guid? Orderid { get; set; }

    public Guid? Productid { get; set; }

    public int? Quantity { get; set; }

    public decimal? Unitprice { get; set; }

    public virtual Ordertable? Order { get; set; }

    public virtual Product? Product { get; set; }
}
