using System.Net;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Mail;

namespace PRN232_EbayClone.Application.Listings.Inventory.Services;

public sealed class InventoryLowStockNotifier : IInventoryLowStockNotifier
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;

    public InventoryLowStockNotifier(IUserRepository userRepository, IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
    }

    public async Task<bool> NotifyIfNeededAsync(
        Domain.Listings.Inventory.Entities.Inventory inventory,
        string listingTitle,
        string listingSku,
        CancellationToken cancellationToken)
    {
        if (!inventory.EmailNotificationsEnabled || !inventory.IsLowStock || inventory.LastLowStockNotificationAt.HasValue)
        {
            return false;
        }

        var seller = await _userRepository.GetByIdAsync(inventory.SellerId, cancellationToken);
        if (seller?.Email is null)
        {
            return false;
        }

        var safeTitle = WebUtility.HtmlEncode(listingTitle);
        var safeSku = WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(listingSku) ? "N/A" : listingSku);
        var thresholdText = inventory.ThresholdQuantity?.ToString() ?? "not set";

        var subject = $"Low stock alert: {listingTitle}";
        var body = $"""
<div style="font-family: Arial, sans-serif; color: #10263f; line-height: 1.6;">
  <h2 style="margin-bottom: 12px;">Low stock alert</h2>
  <p>Hello {WebUtility.HtmlEncode(seller.FullName)},</p>
  <p>Your listing <strong>{safeTitle}</strong> has reached the configured stock threshold.</p>
  <table style="border-collapse: collapse; margin: 16px 0; min-width: 320px;">
    <tr>
      <td style="padding: 8px 12px; border: 1px solid #d9e4ef;"><strong>SKU</strong></td>
      <td style="padding: 8px 12px; border: 1px solid #d9e4ef;">{safeSku}</td>
    </tr>
    <tr>
      <td style="padding: 8px 12px; border: 1px solid #d9e4ef;"><strong>Available quantity</strong></td>
      <td style="padding: 8px 12px; border: 1px solid #d9e4ef;">{inventory.AvailableQuantity}</td>
    </tr>
    <tr>
      <td style="padding: 8px 12px; border: 1px solid #d9e4ef;"><strong>Threshold</strong></td>
      <td style="padding: 8px 12px; border: 1px solid #d9e4ef;">{thresholdText}</td>
    </tr>
    <tr>
      <td style="padding: 8px 12px; border: 1px solid #d9e4ef;"><strong>Reserved quantity</strong></td>
      <td style="padding: 8px 12px; border: 1px solid #d9e4ef;">{inventory.ReservedQuantity}</td>
    </tr>
  </table>
  <p>Please restock or review this listing to avoid missed orders.</p>
</div>
""";

        await _emailSender.SendEmailAsync(seller.Email.Value, subject, body, cancellationToken);
        inventory.MarkLowStockNotificationSent(DateTime.UtcNow);

        return true;
    }
}