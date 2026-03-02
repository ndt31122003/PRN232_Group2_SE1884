namespace PRN232_EbayClone.Domain.Shared.Abstractions;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}
