using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Feedback
{
    public int Id { get; set; }

    public int? Sellerid { get; set; }

    public decimal? Averagerating { get; set; }

    public int? Totalreviews { get; set; }

    public decimal? Positiverate { get; set; }

    public virtual User1? Seller { get; set; }
}
