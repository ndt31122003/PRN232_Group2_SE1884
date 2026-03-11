namespace PRN232_EbayClone.Application.Abstractions.Sms;

public interface ISmsSender
{
    Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
}
