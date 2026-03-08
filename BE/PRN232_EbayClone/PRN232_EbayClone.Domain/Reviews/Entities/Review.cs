using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Review
{
    public Guid Id { get; set; }

    public Guid? Productid { get; set; }

    public Guid? Reviewerid { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User1? Reviewer { get; set; }
}
