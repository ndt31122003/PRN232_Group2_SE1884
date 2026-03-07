using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Coupon
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public decimal? Discountpercent { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public int? Maxusage { get; set; }

    public int? Productid { get; set; }

    public virtual Product? Product { get; set; }
}
