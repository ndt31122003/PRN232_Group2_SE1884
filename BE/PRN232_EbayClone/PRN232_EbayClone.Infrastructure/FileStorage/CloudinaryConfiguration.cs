namespace PRN232_EbayClone.Infrastructure.FileStorage;

public class CloudinaryConfiguration
{
    public required string CloudName { get; set; } = null!;
    public required string ApiKey { get; set; } = null!;
    public required string ApiSecret { get; set; } = null!;
    public string Folder { get; init; } = "ebay-clone";
}
