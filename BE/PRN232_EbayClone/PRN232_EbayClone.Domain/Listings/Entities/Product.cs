using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Product
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? Images { get; set; }

    public int? Categoryid { get; set; }

    public Guid? Sellerid { get; set; }

    public bool? Isauction { get; set; }

    public DateTime? Auctionendtime { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User1? Seller { get; set; }
}
