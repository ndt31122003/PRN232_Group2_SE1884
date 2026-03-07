using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Bid
{
    public Guid Id { get; set; }

    public Guid? Productid { get; set; }

    public Guid? Bidderid { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Bidtime { get; set; }

    public virtual User1? Bidder { get; set; }

    public virtual Product? Product { get; set; }
}
