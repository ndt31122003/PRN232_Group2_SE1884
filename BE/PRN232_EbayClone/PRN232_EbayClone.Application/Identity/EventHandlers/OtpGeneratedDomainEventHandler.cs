using PRN232_EbayClone.Application.Abstractions.Mail;
using PRN232_EbayClone.Application.Abstractions.Sms;
using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Identity.Events;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Application.Identity.EventHandlers;

public sealed class OtpGeneratedDomainEventHandler : INotificationHandler<OtpGeneratedDomainEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IUserRepository _userRepository;

    public OtpGeneratedDomainEventHandler(
        ITemplateRenderer templateRenderer,
        IEmailSender emailSender,
        ISmsSender smsSender,
        IUserRepository userRepository)
    {
        _templateRenderer = templateRenderer;
        _emailSender = emailSender;
        _smsSender = smsSender;
        _userRepository = userRepository;
    }

    public async Task Handle(OtpGeneratedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(new Email(notification.Email), cancellationToken);

        if (notification.Type == OtpType.VerifyPhone)
        {
            if (user?.PhoneNumber is not null)
            {
                var smsMessage = $"Your verification code is {notification.Code}. It expires in {notification.ExpiresInMinutes} minutes.";
                await _smsSender.SendSmsAsync(user.PhoneNumber, smsMessage, cancellationToken);
            }
            return;
        }

        string mailSubject = notification.Type switch
        {
            OtpType.VerifyEmail => "Welcome to PRN232_EbayClone! Please verify your email",
            OtpType.ResetPassword => "PRN232_EbayClone Password Reset Request",
            OtpType.VerifyPayment => "PRN232_EbayClone payment method verification",
            _ => throw new ArgumentOutOfRangeException()
        };

        var mailBody = await _templateRenderer.RenderAsync(
                    "OTP_MAIL",
                    new
                    {
                        AppName = "PRN232_EbayClone",
                        UserName = user?.FullName ?? notification.Email,
                        Code = notification.Code,
                        ExpiresMinutes = notification.ExpiresInMinutes
                    },
                    cancellationToken);

        await _emailSender.SendEmailAsync(
            notification.Email,
            mailSubject,
            mailBody,
            cancellationToken);
    }
}
